using System;
using Harmony;
using UnhollowerBaseLib;
using UnityEngine;

namespace BlanketMod
{
	internal static class Patches
	{
		private const string CLOTH_NAME = "GEAR_Cloth";
		private const string BLANKET_NAME = "GEAR_ClothSheet";
		private const string BEDROLL_NAME = "GEAR_BedRoll";
		private const string FISHING_TACKLE_NAME = "GEAR_HookAndLine";
		private const string SEWINGKIT_NAME = "GEAR_SewingKit";
		private const string BLANKET_CRAFTING_ICON_NAME = "ico_CraftItem__ClothSheet";
		private const string BEDROLL_CRAFTING_ICON_NAME = "ico_CraftItem__BedRoll";

		//Change Preexisting Items
		[HarmonyPatch(typeof(GearItem), "ManualUpdate")]
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
				if (__instance.name == BLANKET_NAME)
				{
					__instance.m_WeightKG = Settings.options.blanketWeight;
					__instance.m_Harvest.m_YieldGearUnits[0] = (int)(Settings.options.blanketWeight * 10);
					__instance.m_Harvest.m_DurationMinutes = ((int)(Settings.options.blanketWeight * 10)) * 10;
				}
			}
		}

		//Change Item Prefabs
		[HarmonyPatch(typeof(GameManager), "Update")]
		private static class ChangePrefabs
		{
			internal static void Postfix()
			{
				GetGearItemPrefab(BEDROLL_NAME).m_WeightKG = Settings.options.bedrollWeight;
				GetGearItemPrefab(BEDROLL_NAME).m_Bed.m_WarmthBonusCelsius = Settings.options.bedrollWarmth;
				GetGearItemPrefab(BLANKET_NAME).m_WeightKG = Settings.options.blanketWeight;
			}
			private static GearItem GetGearItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<GearItem>();
		}

		//Add blanket name and description
		[HarmonyPatch(typeof(LocalizedString), "Text")]
        private static class ChangeDescriptionPatch
        {
            internal static bool Prefix(LocalizedString __instance,ref string __result)
            {
                if (__instance.m_LocalizationID == "GAMEPLAY_ClothSheet")
                {
                    __result = "Patchwork Blanket";
					return false;
                }
				if (__instance.m_LocalizationID == "GAMEPLAY_ClothSheetDescription")
				{
					__result = "An improvised blanket sewn together from scraps of cloth. Will keep you a little warmer on those cold, winter nights.";
					return false;
				}
				return true;
			}
        }

		//Blanket blueprint
		[HarmonyPatch(typeof(GameManager), "Awake")]
		private static class AddBlanketRecipe
		{
			internal static void Postfix()
			{
				BlueprintItem blueprint = GameManager.GetBlueprints().AddComponent<BlueprintItem>();

				// Inputs
				blueprint.m_RequiredGear = new Il2CppReferenceArray<GearItem>(1) { [0] = GetGearItemPrefab(CLOTH_NAME) };
				blueprint.m_RequiredGearUnits = new Il2CppStructArray<int>(1) { [0] = 10 };
				blueprint.m_KeroseneLitersRequired = 0f;
				blueprint.m_GunpowderKGRequired = 0f;
				blueprint.m_RequiredTool = GetToolsItemPrefab(SEWINGKIT_NAME);
				blueprint.m_OptionalTools = new Il2CppReferenceArray<ToolsItem>(1) { [0] = GetToolsItemPrefab(FISHING_TACKLE_NAME) };

				// Outputs
				blueprint.m_CraftedResult = GetGearItemPrefab(BLANKET_NAME);
				blueprint.m_CraftedResultCount = 1;

				// Process
				blueprint.m_Locked = false;
				blueprint.m_AppearsInStoryOnly = false;
				blueprint.m_RequiresLight = true;
				blueprint.m_RequiresLitFire = false;
				blueprint.m_RequiredCraftingLocation = CraftingLocation.Workbench;
				blueprint.m_DurationMinutes = 120;
				blueprint.m_CraftingAudio = "PLAY_CraftingCloth";
				blueprint.m_AppliedSkill = SkillType.None;
				blueprint.m_ImprovedSkill = SkillType.None;
			}

			private static GearItem GetGearItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<GearItem>();
			private static ToolsItem GetToolsItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<ToolsItem>();
		}

		//Bedroll blueprint
		[HarmonyPatch(typeof(GameManager), "Awake")]
		private static class AddBedrollRecipe
		{
			internal static void Postfix()
			{
				BlueprintItem blueprint = GameManager.GetBlueprints().AddComponent<BlueprintItem>();

				// Inputs
				blueprint.m_RequiredGear = new Il2CppReferenceArray<GearItem>(1) { [0] = GetGearItemPrefab(BLANKET_NAME) };
				blueprint.m_RequiredGearUnits = new Il2CppStructArray<int>(1) { [0] = 2 };
				blueprint.m_KeroseneLitersRequired = 0f;
				blueprint.m_GunpowderKGRequired = 0f;
				blueprint.m_RequiredTool = GetToolsItemPrefab(SEWINGKIT_NAME);
				blueprint.m_OptionalTools = new Il2CppReferenceArray<ToolsItem>(1) { [0] = GetToolsItemPrefab(FISHING_TACKLE_NAME) };

				// Outputs
				blueprint.m_CraftedResult = GetGearItemPrefab(BEDROLL_NAME);
				blueprint.m_CraftedResultCount = 1;

				// Process
				blueprint.m_Locked = false;
				blueprint.m_AppearsInStoryOnly = false;
				blueprint.m_RequiresLight = true;
				blueprint.m_RequiresLitFire = false;
				blueprint.m_RequiredCraftingLocation = CraftingLocation.Workbench;
				blueprint.m_DurationMinutes = 120;
				blueprint.m_CraftingAudio = "PLAY_CraftingCloth";
				blueprint.m_AppliedSkill = SkillType.None;
				blueprint.m_ImprovedSkill = SkillType.None;
			}

			private static GearItem GetGearItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<GearItem>();
			private static ToolsItem GetToolsItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<ToolsItem>();
		}

		//Bedroll blueprint from cloth
		[HarmonyPatch(typeof(GameManager), "Awake")]
		private static class AddClothBedrollRecipe
		{
			internal static void Postfix()
			{
				BlueprintItem blueprint = GameManager.GetBlueprints().AddComponent<BlueprintItem>();

				// Inputs
				blueprint.m_RequiredGear = new Il2CppReferenceArray<GearItem>(1) { [0] = GetGearItemPrefab(CLOTH_NAME) };
				blueprint.m_RequiredGearUnits = new Il2CppStructArray<int>(1) { [0] = 18 };
				blueprint.m_KeroseneLitersRequired = 0f;
				blueprint.m_GunpowderKGRequired = 0f;
				blueprint.m_RequiredTool = GetToolsItemPrefab(SEWINGKIT_NAME);
				blueprint.m_OptionalTools = new Il2CppReferenceArray<ToolsItem>(1) { [0] = GetToolsItemPrefab(FISHING_TACKLE_NAME) };

				// Outputs
				blueprint.m_CraftedResult = GetGearItemPrefab(BEDROLL_NAME);
				blueprint.m_CraftedResultCount = 1;

				// Process
				blueprint.m_Locked = false;
				blueprint.m_AppearsInStoryOnly = false;
				blueprint.m_RequiresLight = true;
				blueprint.m_RequiresLitFire = false;
				blueprint.m_RequiredCraftingLocation = CraftingLocation.Workbench;
				blueprint.m_DurationMinutes = 360;
				blueprint.m_CraftingAudio = "PLAY_CraftingCloth";
				blueprint.m_AppliedSkill = SkillType.None;
				blueprint.m_ImprovedSkill = SkillType.None;
			}

			private static GearItem GetGearItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<GearItem>();
			private static ToolsItem GetToolsItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<ToolsItem>();
		}

		[HarmonyPatch(typeof(Panel_Crafting), "ItemPassesFilter")]
		private static class ShowRecipesInToolsSection
		{
			internal static void Postfix(Panel_Crafting __instance, ref bool __result, BlueprintItem bpi)
			{
				if (bpi?.m_CraftedResult?.name == BLANKET_NAME && __instance.m_CurrentCategory == Panel_Crafting.Category.Tools)
				{
					__result = true;
				}
				if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && __instance.m_CurrentCategory == Panel_Crafting.Category.Tools)
				{
					__result = true;
				}
			}
		}

		[HarmonyPatch(typeof(Panel_Crafting), "ItemPassesFilter")]
		private static class UpdateClothRequirements
		{
			internal static void Postfix(BlueprintItem bpi)
			{
				if (bpi?.m_CraftedResult?.name == BLANKET_NAME )
				{
					bpi.m_RequiredGearUnits[0] = (int) (Settings.options.blanketWeight * 10 + 1);
				}
				if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == CLOTH_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int) (Settings.options.bedrollWeight * 10 + 2);
				}
				if (bpi?.m_CraftedResult?.name == BEDROLL_NAME && bpi?.m_RequiredGear[0].name == BLANKET_NAME)
				{
					bpi.m_RequiredGearUnits[0] = (int) ((Settings.options.bedrollWeight / Settings.options.blanketWeight) + 1);
				}
			}
		}

		//Blanket increases Bed Warmth Bonus
		[HarmonyPatch(typeof(Bed),"GetWarmthBonusCelsius")]
		private static class BlanketWarmthBonus
        {
			internal static void Postfix(ref float __result)
            {
				if (GameManager.m_Inventory.GearInInventory(BLANKET_NAME).Length > 0)
				{
					__result += Math.Min( Settings.options.blanketMaxBonus , Settings.options.blanketWarmth * GameManager.m_Inventory.GearInInventory(BLANKET_NAME)[0].m_StackableItem.m_Units );
				}

			}
        }

		[HarmonyPatch(typeof(BlueprintDisplayItem), "Setup")]
		private static class FixRecipeIcons
		{
			internal static void Postfix(BlueprintDisplayItem __instance, BlueprintItem bpi)
			{
				//Blanket Crafting Icon fix
				if (bpi?.m_CraftedResult?.name == BLANKET_NAME)
				{
					Texture2D blanketTexture = Utils.GetCachedTexture(BLANKET_CRAFTING_ICON_NAME);
					if (!blanketTexture)
					{
						blanketTexture = BlanketMod.assetBundle.LoadAsset(BLANKET_CRAFTING_ICON_NAME).Cast<Texture2D>();
						Utils.CacheTexture(BLANKET_CRAFTING_ICON_NAME, blanketTexture);
					}
					__instance.m_Icon.mTexture = blanketTexture;
				}
				//Bedroll Crafting Icon fix
				if (bpi?.m_CraftedResult?.name == BEDROLL_NAME)
				{
					Texture2D bedrollTexture = Utils.GetCachedTexture(BEDROLL_CRAFTING_ICON_NAME);
					if (!bedrollTexture)
					{
						bedrollTexture = BlanketMod.assetBundle.LoadAsset(BEDROLL_CRAFTING_ICON_NAME).Cast<Texture2D>();
						Utils.CacheTexture(BEDROLL_CRAFTING_ICON_NAME, bedrollTexture);
					}
					__instance.m_Icon.mTexture = bedrollTexture;
				}
			}
		}
	}
}