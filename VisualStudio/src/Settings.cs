using ModSettings;
using UnityEngine;

namespace BlanketMod
{
	internal class Settings : JsonModSettings
	{
		public static Settings instance = new Settings();

		[Section("Bedroll")]
		[Name("Weight")]
		[Description("Default is 1 kg. Affects crafting requirements. Setting takes effect on scene change.")]
		[Slider(0.2f, 2f, 19)]
		public float bedrollWeight = 1f;

		[Name("Warmth Bonus Maximum")]
		[Description("The warmth bonus at 100% bedroll condition. Default is 5 celsius. Setting takes effect on scene change.")]
		[Slider(0f, 10f, 101)]
		public float bedrollWarmth = 5f;

		[Section("Blanket")]
		[Name("Weight")]
		[Description("Recommended is 0.6 kg. Affects crafting requirements. Setting takes effect on scene change.")]
		[Slider(0.2f, 1.5f, 14)]
		public float blanketWeight = .6f;

		[Name("Warmth Bonus")]
		[Description("The additional warmth bonus per blanket carried. Recommended is 1 celsius.")]
		[Slider(0f, 5f, 51)]
		public float blanketWarmth = 1f;

		[Name("Max Bonus")]
		[Description("The maximum additional warmth a player can have. Recommended is 5 celsius.")]
		[Slider(0f, 20f, 201)]
		public float blanketMaxBonus = 5f;

		protected override void OnConfirm()
		{
			base.OnConfirm();
			ChangePrefabWeights();
		}
		internal void ChangePrefabWeights()
		{
			GetGearItemPrefab("GEAR_BedRoll").m_WeightKG = Settings.instance.bedrollWeight;
			GetGearItemPrefab("GEAR_BedRoll").m_Bed.m_WarmthBonusCelsius = Settings.instance.bedrollWarmth;
			GetGearItemPrefab("GEAR_ClothSheet").m_WeightKG = Settings.instance.blanketWeight;
		}
		private static GearItem GetGearItemPrefab(string name) => Resources.Load(name).Cast<GameObject>().GetComponent<GearItem>();
	}
}