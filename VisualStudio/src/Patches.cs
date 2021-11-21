using HarmonyLib;
using System;

namespace BlanketMod
{
	internal static class Patches
	{
		private const string CLOTH_NAME = "GEAR_Cloth";
		private const string BLANKET_NAME = "GEAR_ClothSheet";
		private const string BEDROLL_NAME = "GEAR_BedRoll";

		//Change Preexisting Items
		[HarmonyPatch(typeof(GearItem), "Awake")]
		private static class ChangeItems
		{
			internal static void Postfix(GearItem __instance)
			{
				if (__instance.name == BEDROLL_NAME)
				{
					__instance.m_WeightKG = Settings.instance.bedrollWeight;
					__instance.m_Harvest.m_YieldGearUnits[0] = (int)(Settings.instance.bedrollWeight * 10);
					__instance.m_Harvest.m_DurationMinutes = ((int)(Settings.instance.bedrollWeight * 10)) * 10;
					__instance.m_Bed.m_WarmthBonusCelsius = Settings.instance.bedrollWarmth;
				}
				else if (__instance.name == BLANKET_NAME)
				{
					__instance.m_WeightKG = Settings.instance.blanketWeight;
					__instance.m_Harvest.m_YieldGearUnits[0] = (int)(Settings.instance.blanketWeight * 10);
					__instance.m_Harvest.m_DurationMinutes = ((int)(Settings.instance.blanketWeight * 10)) * 10;
				}
			}
		}

		[HarmonyPatch(typeof(BlueprintDisplayItem), "Setup")]
		private static class UpdateClothRequirements
		{
			internal static void Postfix(BlueprintItem bpi)
			{
				if (bpi?.m_CraftedResult?.name == BLANKET_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)(Settings.instance.blanketWeight * 10 + 1);
				}
				else if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == CLOTH_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)(Settings.instance.bedrollWeight * 10 + 2);
				}
				else if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == BLANKET_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int)((Settings.instance.bedrollWeight / Settings.instance.blanketWeight) + 1);
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
					__result += Math.Min(Settings.instance.blanketMaxBonus, Settings.instance.blanketWarmth * GameManager.m_Inventory.GearInInventory(BLANKET_NAME)[0].m_StackableItem.m_Units);
				}
			}
		}
	}
}