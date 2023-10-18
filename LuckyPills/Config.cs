namespace LuckyPills
{
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string PickupMessage { get; set; } = "You have picked up SCP-5854";

        // Code used from https://github.com/NutInc/LuckyPillsRework/blob/master/LuckyPillsRework/Config.cs
        public Dictionary<ProjectileType, float> VomitIntervals { get; set; } = new()
        {
            { ProjectileType.Scp018, 5 },
            { ProjectileType.FragGrenade, 0.1f },
            { ProjectileType.Flashbang, 0.1f },
        };

        [Description("The amount of damage dealt to the player with the specified vomit effect.")]
        public Dictionary<ProjectileType, int> VomitDamage { get; set; } = new()
        {
            { ProjectileType.Scp018, 1 },
            { ProjectileType.Flashbang, 1 },
            { ProjectileType.FragGrenade, 0 },
        };

        [Description("This Changes the MovementBoost Intensity. MAX: 255")]
        public byte MovementBoostIntensity { get; set; } = 255;

        [Description("Remove or re-add Possible Effects")]
        public List<string> PossibleEffects { get; set; } = new()
        {
            "explode",
            "mutate",
            "god",
            "paper",
            "upsidedown",
            "bombvomit",
            "flashvomit",
            "scp268",
            "bleeding",
            "corroding",
            "decontaminating",
            "hemorrhage",
            "sinkhole",
            "sizedown",
            "tpscp",
            "sonic",
            "basketballplayer",
            "deafened",
            "blinded",
            "rndtp",
            "tptoply",
            "dropitems",
            "tantrum"
        };

        [Description("Change duration of effects")]
        public float MinDuration { get; set; } = 5f;

        public float MaxDuration { get; set; } = 30f;
    }
}