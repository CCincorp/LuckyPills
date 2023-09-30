namespace LuckyPills
{
    using Exiled.API.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string PickupMessage { get; set; } = "You have picked up SCP-5854";

        [Description("Change often a Grenade, Ball or Flashbang should be thrown")]
        public float FlashVomitInterval { get; set; } = 0.1f;
        public float BallVomitInterval { get; set; } = 0.2f;
        public float GrenadeVomitInterval { get; set; } = 0.1f;

        [Description("How much Damage should be given to the Player when Ball, Grenade, Flash Vomit")]
        public float DamageOnThrow { get; set; } = 1f;

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