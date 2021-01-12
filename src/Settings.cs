using ModSettings;

namespace BlanketMod
{
    internal class BlanketModSettings : JsonModSettings
    {
        [Section("Bedroll")]
        [Name("Weight")]
        [Description("Default is 1 kg. Recommended is 1.5 kg.")]
        [Slider(0f, 2f, 21)]
        public float bedrollWeight = 1.5f;

        [Name("Warmth Bonus Maximum")]
        [Description("The warmth bonus at 100% bedroll condition. Default is 5 celsius.")]
        [Slider(0f, 10f, 101)]
        public float bedrollWarmth = 5f;

        [Section("Blanket")]
        [Name("Weight")]
        [Description("Recommended is 0.8 kg.")]
        [Slider(0f, 1.5f, 16)]
        public float blanketWeight = .8f;

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