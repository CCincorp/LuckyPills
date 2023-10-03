using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using MEC;
using CustomPlayerEffects;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Exiled.API.Features.Roles;
using System;

namespace LuckyPills
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        private readonly Config config;

        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
            this.config = plugin.Config;
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
            if (ev.Item.Type == ItemType.Painkillers)
            {
                Timing.RunCoroutine(RunPillCoroutine(ev));
            }
        }

        private IEnumerator<float> RunPillCoroutine(UsedItemEventArgs ev)
        {
            Item item = ev.Item;
            Player player = ev.Player;

            if (player.IsInPocketDimension) yield break;

            string effectType = NextEffect();
            float duration = Mathf.Ceil(UnityEngine.Random.Range(config.MinDuration, config.MaxDuration));

            config.EffectHints.TryGetValue(effectType, out string value);

            Log.Debug($"Player: {player.UserId}, Effect: {effectType}, Duration: {duration}");

            player.RemoveItem(item);

            switch(effectType)
            {
                case "bleeding":
                    player.EnableEffect<Bleeding>(duration);
                    
                    value = String.Format(value, duration);

                    break;
                case "bombvomit":
                    Timing.RunCoroutine(Vomit(player, ProjectileType.FragGrenade, duration));
                    value = String.Format(value, duration);

                    break;
                case "ballvomit":
                    Timing.RunCoroutine(Vomit(player, ProjectileType.Scp018, duration));
                    value = String.Format(value, duration);

                    break;
                case "corroding":
                    player.EnableEffect<PocketCorroding>(duration);
                    value = String.Format(value, duration);

                    break;
                case "decontaminating":
                    player.EnableEffect<Decontaminating>(duration);
                    value = String.Format(value, duration);

                    break;
                case "explode":
                    ExplosiveGrenade explosiveGrenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;       
                    explosiveGrenade.FuseTime = .5f;
                    explosiveGrenade.SpawnActive(ev.Player.Position);

                    break;
                case "flashed":
                    FlashGrenade flashGrenade = Item.Create(ItemType.GrenadeFlash) as FlashGrenade;
                    flashGrenade.FuseTime = .5f;
                    flashGrenade.SpawnActive(ev.Player.Position);

                    break;
                case "flashvomit":
                    Timing.RunCoroutine(Vomit(player, ProjectileType.Flashbang, duration));
                    value = String.Format(value, duration);

                    break;
                case "god":
                    player.IsGodModeEnabled = true;
                    Timing.CallDelayed(duration, () => player.IsGodModeEnabled = false);
                    value = String.Format(value, duration);

                    break;
                case "hemorrhage":
                    player.EnableEffect<Hemorrhage>(duration);
                    value = String.Format(value, duration);

                    break;
                case "mutate":
                    Role cachedMutatorRole = player.Role;

                    player.DropItems();
                    player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass);

                    Vector3 cachedPos = player.Position;

                    Timing.CallDelayed(duration, () => 
                    {
                        player.Role.Set(cachedMutatorRole, SpawnReason.ForceClass);

                        player.Position = cachedPos;
                    });

                    value = String.Format(value, duration);

                    break;
                case "paper":
                    player.Scale = new(1f, 1f, 0.01f);
                    Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));
                    value = String.Format(value, duration);

                    break;
                case "sinkhole":
                    player.EnableEffect<Sinkhole>(duration);
                    value = String.Format(value, duration);

                    break;
                case "scp268":
                    player.EnableEffect<Invisible>(duration);

                    value = String.Format(value, duration);

                    break;
                case "upsidedown":
                    player.Scale = new(1f, -1f, 1f);
                    Timing.CallDelayed(duration, () =>
                    {
                        player.Scale = new(1f, 1f, 1f);
                        player.Position += new Vector3(0, 1, 0);
                    });

                    value = String.Format(value, duration);

                    break;
                case "sizedown":
                    player.Scale = new(0.5f, 0.5f, 0.5f);
                    Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));

                    value = String.Format(value, duration);
                    
                    break;
                case "tpscp":
                    if (Player.Get(Side.Scp).Any())
                    {
                        Player scpPlayer = Player.Get(Side.Scp).ToList().RandomItem();
                        player.Position = scpPlayer.Position;
                    }
                    else
                    {
                        config.EffectHints.TryGetValue("tpscpfailed", out value);
                    }
                    break;
                case "sonic":
                    player.EnableEffect<MovementBoost>(duration);
                    player.ChangeEffectIntensity<MovementBoost>(config.MovementBoostIntensity, duration);

                    value = String.Format(value, duration);
                    break;
                case "basketballplayer":
                    player.Scale = new Vector3(1.5f, 1.5f, 1.5f);

                    Timing.CallDelayed(duration, () => player.Scale = new Vector3(1f, 1f, 1f));

                    value = String.Format(value, duration);

                    break;
                case "deafened":
                    player.EnableEffect<Deafened>(duration);

                    value = String.Format(value, duration);
                    break;
                case "blinded":
                    player.EnableEffect<Blinded>(duration);

                    value = String.Format(value, duration);
                    break;
                case "rndtp":
                    Room rand = Room.List.ElementAt(UnityEngine.Random.Range(0, Room.List.Count()));
                    player.Position = rand.Position + Vector3.up;

                    break;
                case "tptoply":
                    {
                        List<Player> players = Player.List.Where(p => p.IsScp == false && p.IsAlive == true && p != player).ToList();
                        if (players.Count() == 0)
                        {
                            config.EffectHints.TryGetValue("tptoplyfailed", out value);
                        }
                        else
                        {
                            Player ply = players.ElementAt(UnityEngine.Random.Range(0, players.Count()));
                            player.Position = ply.Position + Vector3.up;
                            value = String.Format(value, ply.Nickname);
                        }
                        break;
                    }
                case "dropitems":
                    player.DropItems();

                    break;
                case "tantrum":
                    player.PlaceTantrum();

                    break;
                default:
                    player.ShowHint($"Unknown Effect: {effectType}");
                    break;
            }
            player.ShowHint(value);
        }

        private string NextEffect() => plugin.Config.PossibleEffects[UnityEngine.Random.Range(0, config.PossibleEffects.Count)];
    }
}
