namespace LuckyPills
{
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;
    using InventorySystem.Items.Usables.Scp330;
    using PlayerRoles;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string PickupMessage { get; set; } = "You have picked up SCP-5854";

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

        [Description("If player gets IncreaseHealth effect this will decide by how much")]
        public float IncreaseHealh { get; set; } = 1.5f;

        [Description("Set MovementBoostIntensity for effect Sonic")]
        public byte MovementBoostIntensity { get; set; } = 255;

        [Description("Allows the player to use the pills in the Pocket Dimension")]
        public bool AllowInPocketDimension { get; set; } = true;

        [Description("List: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> BadEffectTypes { get; set; } = new()
        {
            EffectType.Asphyxiated,
            EffectType.Bleeding,
            EffectType.Blinded,
            EffectType.Burned,
            EffectType.Concussed,
            EffectType.Corroding,
            EffectType.CardiacArrest,
            EffectType.Deafened,
            EffectType.Decontaminating,
            EffectType.Disabled,
            EffectType.Ensnared,
            EffectType.Exhausted,
            EffectType.Flashed,
            EffectType.Hemorrhage,
            EffectType.Hypothermia,
            EffectType.InsufficientLighting,
            EffectType.Poisoned,
            EffectType.PocketCorroding,
            EffectType.SeveredHands,
            EffectType.SinkHole,
            EffectType.Stained,
            EffectType.Traumatized
        };

        [Description("List: https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> GoodEffectTypes { get; set; } = new()
        {
            EffectType.BodyshotReduction,
            Effecttype.AntiScp207,
            EffectType.DamageReduction,
            EffectType.Invigorated,
            EffectType.Invisible,
            EffectType.RainbowTaste,
            EffectType.Scp1853,
            EffectType.Scp207,
            EffectType.Vitality
        };

        [Description("Items receivable from randomitem effect")]
        public HashSet<ItemType> RandomItemList { get; set; } = new()
        {
            ItemType.Adrenaline,
            ItemType.Coin,
            ItemType.Flashlight,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Radio,
            ItemType.SCP207,
            ItemType.SCP2176,
            ItemType.SCP244a,
            ItemType.SCP268,
            ItemType.SCP500,
            ItemType.GrenadeFlash,
            ItemType.GrenadeHE,
            ItemType.GunA7,
            ItemType.GunAK,
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunCom45,
            ItemType.GunCrossvec,
            ItemType.GunE11SR,
            ItemType.GunFRMG0,
            ItemType.GunFSP9,
            ItemType.GunRevolver,
            ItemType.GunShotgun,
            ItemType.SCP018,
        };

        [Description("Roles possible to become from randomteam effect")]
        public HashSet<RoleTypeId> PossableRoles { get; set; } = new()
        {
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.FacilityGuard,
        };

        [Description("Good Possible Effects")]
        public List<string> GoodEffects { get; set; } = new()
        {
            "randomitem",
            "randomteam",
            "goodeffect",
            "increasehealth",
            "sonic",
            "god",
            "flipnukeswitch",
            "bypass",
            "ap",
            "primitive",
            "pinkcandy",
            "tptoply",

        };

        [Description("Bad Possible Effects")]
        public List<string> BadEffects { get; set; } = new()
        {
            "grenade",
            "mutate",
            "paper",
            "bombvomit",
            "bombvomit",
            "bombvomit",
            "flashvomit",
            "sizedown",
            "tpscp",
            "basketballplayer",
            "rndtp",
            "dropitems",
            "tantrum",
            "badeffect",
            "grandma",
        };

        [Description("Change Chances for a Bad effect. e.g. if ChanceForBadEffect is 20 then a chance for a good Effect are 80 and so on")]
        public int ChanceForBadEffect { get; set; } = 20;

        [Description("Change duration of effects")]
        public float MinDuration { get; set; } = 5f;

        public float MaxDuration { get; set; } = 30f;
    }
}
