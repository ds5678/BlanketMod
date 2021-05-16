using Harmony;
using System;

namespace BlanketMod
{
	internal static class Patches
	{
		private const string CLOTH_NAME = "GEAR_Cloth";
		private const string BLANKET_NAME = "GEAR_ClothSheet";
		private const string BEDROLL_NAME = "GEAR_BedRoll";
		private const string BLANKET_CRAFTING_ICON_NAME = "ico_CraftItem__ClothSheet";
		private const string BEDROLL_CRAFTING_ICON_NAME = "ico_CraftItem__BedRoll";

		//Change Preexisting Items
		[HarmonyPatch(typeof(GearItem), "Awake")]
		private static class ChangeItems
		{
			internal static void Postfix(GearItem __instance)
			{
				if (__instance.name == BEDROLL_NAME)
				{
					__instance.m_WeightKG = Settings.options.bedrollWeight;
					__instance.m_Harvest.m_YieldGearUnits[0] = (int)(Settings.options.bedrollWeight * 10);
					__instance.m_Harvest.m_DurationMinutes = ((int)(Settings.options.bedrollWeight * 10)) * 10;
					__instance.m_Bed.m_WarmthBonusCelsius = Settings.options.bedrollWarmth;
				}
				else if (__instance.name == BLANKET_NAME)
				{
					__instance.m_WeightKG = Settings.options.blanketWeight;
					__instance.m_Harvest.m_YieldGearUnits[0] = (int)(Settings.options.blanketWeight * 10);
					__instance.m_Harvest.m_DurationMinutes = ((int)(Settings.options.blanketWeight * 10)) * 10;
				}
			}
		}

		[HarmonyPatch(typeof(Panel_Crafting), "ItemPassesFilter")]
		private static class ShowRecipesInToolsSection
		{
			internal static void Postfix(Panel_Crafting __instance, ref bool __result, BlueprintItem bpi)
			{
				if (__instance.m_CurrentCategory == Panel_Crafting.Category.Tools)
				{
					if (bpi?.m_CraftedResult?.name == BEDROLL_NAME || bpi?.m_CraftedResult?.name == BLANKET_NAME)
					{
						__result = true;
					}
				}
			}
		}

		[HarmonyPatch(typeof(Panel_Crafting), "ItemPassesFilter")]
		private static class UpdateClothRequirements
		{
			internal static void Postfix(BlueprintItem bpi)
			{
				if (bpi?.m_CraftedResult?.name == BLANKET_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)(Settings.options.blanketWeight * 10 + 1);
				}
				else if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == CLOTH_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)(Settings.options.bedrollWeight * 10 + 2);
				}
				else if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == BLANKET_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)((Settings.options.bedrollWeight / Settings.options.blanketWeight) + 1);
				}
			}
		}

		//Blanket increases Bed Warmth Bonus
		[HarmonyPatch(typeof(Bed), "GetWarmthBonusCelsius")]
		private static class BlanketWarmthBonus
		{
			internal static void Postfix(ref float __result)
			{
				if (GameManager.m_Inventory.GearInInventory(BLANKET_NAME).Length > 0)
				{
					__result += Math.Min(Settings.options.blanketMaxBonus, Settings.options.blanketWarmth * GameManager.m_Inventory.GearInInventory(BLANKET_NAME)[0].m_StackableItem.m_Units);
				}
			}
		}
	}
}