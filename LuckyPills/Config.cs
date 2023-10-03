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
        public Dictionary<ProjectileType, float> VomitIntervals { get; set; } = new Dictionary<ProjectileType, float>()
        {
            { ProjectileType.Scp018, 5 },
            { ProjectileType.FragGrenade, 0.1f },
            { ProjectileType.Flashbang, 0.1f },
        };

        [Description("The amount of damage dealt to the player with the specified vomit effect.")]
        public Dictionary<ProjectileType, int> VomitDamage { get; set; } = new Dictionary<ProjectileType, int>()
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

        [Description("Set a different hint message. NOTE: {0} will be replaced with the seconds or player's name depending on the effect")]
        public Dictionary<string, string> EffectHints { get; set; } = new()
        {
            {"bleeding", "You've been given bleeding for {0} seconds"},
            {"bombvomit",  "You've been given bomb vomit for {0} seconds"},
            {"ballvomit",  "You've been given ball vomit for {0} seconds"},
            {"corroding",  "You've been sent to the pocket dimension"},
            {"decontaminating",  "You've been given decontamination for {0} seconds"},
            {"explode",  "You've been exploded"},
            {"flashed",  "You've been flashed"},
            {"flashvomit",  "You've been given flash vomit for {0} seconds"},
            {"god",  "You've been given god mode for {0} seconds"},
            {"hemorrhage",  "You've been hemorrhaged for {0} seconds"},
            {"mutate",  "You've been mutated for {0} seconds"},
            {"paper",  "You've been turned into paper for {0} seconds"},
            {"sinkhole",  "You've been given sinkhole effect for {0} seconds"},
            {"scp268",  "You've been turned invisible for {0} seconds"},
            {"upsidedown",  "You've been converted to Australian for {0} seconds"},
            {"sizedown",  "You've been cut in half for {0} seconds"},
            {"tpscp",  "You've been teleported to an SCP."},
            {"tpscpfailed",  "No SCP found to teleport to."},
            {"sonic",  "You've been made in to Sonic for {0} seconds"},
            {"basketballplayer",  "You've been converted to a basketball player for {0} seconds"},
            {"deafened",  "You've been Deafened for {0} seconds"},
            {"blinded",  "You've been Blinded for {0} seconds"},
            {"rndtp",  "You've been teleported to a random room."},
            {"tptoplyfailed",  "No Player found to teleport."},
            {"tptoply",  "You've been teleported to {0}"},
            {"dropitems",  "It looks like you dropped something."},
            {"tantrum",  "It looks like you didn't get to the toilet in time."},
        };

        [Description("Change duration of effects")]
        public float MinDuration { get; set; } = 5f;

        public float MaxDuration { get; set; } = 30f;
    }
}