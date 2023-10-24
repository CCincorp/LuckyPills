using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using CustomPlayerEffects;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Exiled.API.Features.Roles;
using Exiled.API.Extensions;
using Exiled.API.Features.Toys;
using InventorySystem.Items.Usables.Scp330;

namespace LuckyPills
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        private readonly Config config;
        private readonly Translation translation;

        private System.Random rnd = new System.Random();

        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
            this.config = plugin.Config;
            this.translation = plugin.Translation;
        }

        private static void SpawnGrenadeOnPlayer(Player player, ProjectileType grenadeType)
        {
            switch (grenadeType)
            {
                case ProjectileType.Scp018:
                    Throwable throwable = Item.Create(ItemType.SCP018, player) as Throwable;

                    throwable.Throw(true);

                    break;
                default:
                    player.ThrowGrenade(grenadeType, true);

                    break;
            }
        }

        private IEnumerator<float> Vomit(Player player, ProjectileType projectile, float timer)
        {
            for (var i = 0; i < timer * 10.0 && player.IsAlive; ++i)
            {
                yield return Timing.WaitForSeconds(config.VomitIntervals[projectile]);
                player.Hurt(config.VomitDamage[projectile]);

                SpawnGrenadeOnPlayer(player, projectile);
            }
        }

        public void OnPickup(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Type == ItemType.Painkillers)
            {
                ev.Player.ShowHint(plugin.Config.PickupMessage);
            }
        }

        public void UsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Painkillers) return;

            string goodorbad = "good";

            if (rnd.Next(1,101) <= config.ChanceForBadEffect)
            {
                goodorbad = "bad";
            }

            Timing.RunCoroutine(RunPillCoroutine(ev, goodorbad));
        }

        private IEnumerator<float> RunPillCoroutine(UsedItemEventArgs ev, string badorgood)
        {
            Item item = ev.Item;
            Player player = ev.Player;

            string effectType = NextEffect(badorgood);
            float duration = Mathf.Ceil(UnityEngine.Random.Range(config.MinDuration, config.MaxDuration));

            translation.EffectHints.TryGetValue(effectType, out string value);

            if (!config.AllowInPocketDimension && ev.Player.IsInPocketDimension) yield break;

            Log.Debug($"Player: {player.UserId} | {player.Nickname} | {player.Id}, Effect: {effectType}");

            player.RemoveItem(item);

            switch (effectType)
            {
                case "bombvomit":
                    {
                        Timing.RunCoroutine(Vomit(player, ProjectileType.FragGrenade, duration));
                        value = string.Format(value, duration);
                    }
                    break;
                case "ballvomit":
                    {
                        Timing.RunCoroutine(Vomit(player, ProjectileType.Scp018, duration));
                        value = string.Format(value, duration);
                    }
                    break;
                case "grenade":
                    {
                        ExplosiveGrenade explosiveGrenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;
                        explosiveGrenade.FuseTime = 2f;
                        explosiveGrenade.SpawnActive(ev.Player.Position);
                    }
                    break;
                case "flashed":
                    {
                        FlashGrenade flashGrenade = Item.Create(ItemType.GrenadeFlash) as FlashGrenade;
                        flashGrenade.FuseTime = .5f;
                        flashGrenade.SpawnActive(ev.Player.Position);
                    }
                    break;
                case "flashvomit":
                    {
                        Timing.RunCoroutine(Vomit(player, ProjectileType.Flashbang, duration));
                        value = string.Format(value, duration);
                    }
                    break;
                case "god":
                    {
                        player.IsGodModeEnabled = true;
                        Timing.CallDelayed(duration, () => player.IsGodModeEnabled = false);
                        value = string.Format(value, duration);
                    }
                    break;
                case "mutate":
                    {
                        Role cachedMutatorRole = player.Role;

                        player.DropItems();
                        player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass);

                        Vector3 cachedPos = player.Position;

                        Timing.CallDelayed(duration, () =>
                        {
                            player.Role.Set(cachedMutatorRole, SpawnReason.ForceClass);

                            player.Position = cachedPos;
                        });

                        value = string.Format(value, duration);
                    }
                    break;
                case "paper":
                    {
                        player.Scale = new(1f, 1f, 0.01f);
                        Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));
                        value = string.Format(value, duration);
                    }
                    break;
                case "sizedown":
                    {
                        player.Scale = new(0.5f, 0.5f, 0.5f);
                        Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));
                        value = string.Format(value, duration);
                    }

                    break;
                case "tpscp":
                    {
                        if (Player.Get(Side.Scp).Any())
                        {
                            Player scpPlayer = Player.Get(Side.Scp).ToList().RandomItem();
                            player.Position = scpPlayer.Position;

                            value = string.Format(value, scpPlayer.Role.Name);
                        }
                        else
                        {
                            translation.EffectHints.TryGetValue("tpscpfailed", out value);
                        }
                    }
                    break;
                case "sonic":
                    {
                        player.EnableEffect<MovementBoost>(duration);
                        player.ChangeEffectIntensity<MovementBoost>(config.MovementBoostIntensity, duration);

                        value = string.Format(value, duration);
                    }
                    break;
                case "basketballplayer":
                    {
                        player.Scale = new Vector3(1.5f, 1.5f, 1.5f);

                        Timing.CallDelayed(duration, () => player.Scale = new Vector3(1f, 1f, 1f));

                        value = string.Format(value, duration);
                    }
                    break;
                case "rndtp":
                    {
                        if (Warhead.IsDetonated)
                        {
                            translation.EffectHints.TryGetValue("rndtpwarhead", out value);
                            break;
                        }

                        Room room = Room.List.ElementAt(UnityEngine.Random.Range(0, Room.List.Count()));

                        if (Map.IsLczDecontaminated)
                        {
                            var rooms = Room.List.Where(r => r.Zone != ZoneType.LightContainment);

                            room = rooms.ElementAt(UnityEngine.Random.Range(0, rooms.Count()));
                        }

                        player.Position = room.Position + Vector3.up;
                    }
                    break;
                case "tptoply":
                    {
                        List<Player> players = Player.List.Where(p => p.IsScp == false && p.IsAlive == true && p != player).ToList();
                        if (players.Count() == 0)
                        {
                            translation.EffectHints.TryGetValue("tptoplyfailed", out value);
                        }
                        else
                        {
                            Player ply = players.ElementAt(UnityEngine.Random.Range(0, players.Count()));
                            player.Position = ply.Position + Vector3.up;
                            value = string.Format(value, ply.Nickname);
                        }
                        break;
                    }
                case "dropitems":
                    {
                        player.DropItems();
                    }
                    break;
                case "tantrum":
                    {
                        player.PlaceTantrum();
                    }
                    break;
                case "badeffect":
                    {
                        EffectType effect = config.BadEffectTypes.GetRandomValue();

                        player.EnableEffect(effect, duration);

                        value = string.Format(value, effect, duration);
                    }
                    break;
                case "goodeffect":
                    {
                        EffectType effect = config.GoodEffectTypes.GetRandomValue();

                        player.EnableEffect(effect, duration);

                        value = string.Format(value, effect, duration);
                    }
                    break;
                case "increasehealth":
                    {
                        player.Health *= config.IncreaseHealh;
                    }
                    break;
                case "grandma":
                    {
                        Scp244 scp224 = Item.Create(ItemType.SCP244a) as Scp244;

                        scp224.Primed = true;

                        scp224.CreatePickup(player.Position);
                    }
                    break;
                case "randomitem":
                    {
                        ItemType itemType = config.RandomItemList.GetRandomValue();

                        player.AddItem(itemType);

                        value = string.Format(value, itemType);
                    }
                    break;
                case "randomteam":
                    {
                        player.DropItems();

                        RoleTypeId roleType = config.PossableRoles.GetRandomValue();

                        player.Role.Set(roleType, SpawnReason.ForceClass);

                        value = string.Format(value, roleType.GetFullName());
                    }
                    break;
                case "flipnukeswitch":
                    {
                        if (Warhead.LeverStatus == true)
                        {
                            Warhead.LeverStatus = false;
                            translation.EffectHints.TryGetValue("flipnukeswitchOFF", out value);
                            break;
                        }

                        Warhead.LeverStatus = true;
                        translation.EffectHints.TryGetValue("flipnukeswitchON", out value);
                    }
                    break;
                case "bypass":
                    player.IsBypassModeEnabled = true;
                    Timing.CallDelayed(duration, () => player.IsBypassModeEnabled = false);

                    value = string.Format(value, duration);
                    break;
                case "ap":
                    {
                        player.AddAhp(100, limit: 100, decay: 0);
                    }
                    break;
                case "primitive":
                    {
                        Primitive.Create(player.Position, scale: Vector3.one);
                    }
                    break;
                case "pinkcandy":
                    {
                        Scp330 scp330 = Item.Create(ItemType.SCP330) as Scp330;

                        scp330.AddCandy(CandyKindID.Pink);

                        scp330.CreatePickup(player.Position);
                    }
                    break;
                default:
                    translation.EffectHints.TryGetValue("UnknownEffect", out value);

                    value = string.Format(value, effectType);
                    break;
            }
            player.ShowHint(value);
        }

        private string NextEffect(string goodorbad)
        {
            if (goodorbad == "bad") 
            {
                return plugin.Config.BadEffects[UnityEngine.Random.Range(0, config.BadEffects.Count)];
            }
            return plugin.Config.GoodEffects[UnityEngine.Random.Range(0, config.GoodEffects.Count)];
        }
    }
}
