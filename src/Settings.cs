using ModSettings;

namespace BlanketMod
{
    internal class BlanketModSettings : JsonModSettings
    {
        [Section("Bedroll")]
        [Name("Weight")]
        [Description("Default is 1 kg. Affects crafting requirements.")]
        [Slider(0.2f, 2f, 19)]
        public float bedrollWeight = 1f;

        [Name("Warmth Bonus Maximum")]
        [Description("The warmth bonus at 100% bedroll condition. Default is 5 celsius.")]
        [Slider(0f, 10f, 101)]
        public float bedrollWarmth = 5f;

        [Section("Blanket")]
        [Name("Weight")]
        [Description("Recommended is 0.6 kg. Affects crafting requirements.")]
        [Slider(0.2f, 1.5f, 14)]
        public float blanketWeight = .6f;

        [Name("Warmth Bonus")]
        [Description("The additional warmth bonus per blanket carried. Recommended is 1 celsius.")]
        [Slider(0f, 2f, 21)]
        public float blanketWarmth = 1f;

        [Name("Max Bonus")]
        [Description("The maximum additional warmth a player can have. Recommended is 5 celsius.")]
        [Slider(0f, 10f, 101)]
        public float blanketMaxBonus = 5f;
    }
    internal static class Settings
    {
        public static BlanketModSettings options;
        public static void OnLoad()
        {
            options = new BlanketModSettings();
            options.AddToModSettings("Blanket Mod Settings");
        }
    }
}