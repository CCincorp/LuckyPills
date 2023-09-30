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

        private IEnumerator<float> GrenadeVomitTime(Player player, float randomTimer)
        {
            for (var i = 0; i < randomTimer * 10.0 && player.IsAlive; ++i)
            {
                yield return Timing.WaitForSeconds(plugin.Config.GrenadeVomitInterval);
                SpawnGrenadeOnPlayer(player, ProjectileType.FragGrenade);
            }
        }

        private IEnumerator<float> FlashVomitTime(Player player, float randomTimer)
        {
            for (var i = 0; i < randomTimer * 10.0 && player.IsAlive; ++i)
            {
                yield return Timing.WaitForSeconds(plugin.Config.FlashVomitInterval);
                player.Hurt(config.DamageOnThrow);
                SpawnGrenadeOnPlayer(player, ProjectileType.Flashbang);
            }
        }

        private IEnumerator<float> BallVomitTime(Player player, float randomTimer)
        {
            for (var i = 0; i < randomTimer * 10.0 && player.IsAlive; ++i)
            {
                yield return Timing.WaitForSeconds(plugin.Config.BallVomitInterval);
                player.Hurt(config.DamageOnThrow);
                SpawnGrenadeOnPlayer(player, ProjectileType.Scp018);
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
            yield return Timing.WaitForSeconds(0.1f);
            
            Item item = ev.Item;
            Player player = ev.Player;

            string effectType = NextEffect();
            float duration = Mathf.Ceil(Random.Range(config.MinDuration, config.MaxDuration));

            Log.Debug($"Player: {player.UserId}, Effect: {effectType}, Duration: {duration}");

            player.RemoveItem(item);

            switch(effectType)
            {
                case "bleeding":
                    player.EnableEffect<Bleeding>(duration);
                    player.ShowHint($"You've been given bleeding for {duration} seconds");

                    break;
                case "bombvomit":
                    Timing.RunCoroutine(GrenadeVomitTime(player, duration));
                    player.ShowHint($"You've been given bomb vomit for {duration} seconds");

                    break;
                case "ballvomit":
                    Timing.RunCoroutine(BallVomitTime(player, duration));
                    player.ShowHint($"You've been given ball vomit for {duration} seconds");

                    break;
                case "corroding":
                    player.EnableEffect<PocketCorroding>(duration);
                    player.ShowHint("You've been sent to the pocket dimension");

                    break;
                case "decontaminating":
                    player.EnableEffect<Decontaminating>(duration);
                    player.ShowHint($"You've been given decontamination for {duration} seconds");

                    break;
                case "explode":
                    ExplosiveGrenade explosiveGrenade = Item.Create(ItemType.GrenadeHE) as ExplosiveGrenade;       
                    explosiveGrenade.FuseTime = .5f;
                    explosiveGrenade.SpawnActive(ev.Player.Position);

                    player.ShowHint("You've been exploded");

                    break;
                case "flashed":
                    FlashGrenade flashGrenade = Item.Create(ItemType.GrenadeFlash) as FlashGrenade;
                    flashGrenade.FuseTime = .5f;
                    flashGrenade.SpawnActive(ev.Player.Position);

                    player.ShowHint("You've been flashed");

                    break;
                case "flashvomit":
                    Timing.RunCoroutine(FlashVomitTime(player, duration));
                    player.ShowHint($"You've been given flash vomit for {duration} seconds");

                    break;
                case "god":
                    player.IsGodModeEnabled = true;
                    Timing.CallDelayed(duration, () => player.IsGodModeEnabled = false);
                    player.ShowHint($"You've been given god mode for {duration} seconds");

                    break;
                case "hemorrhage":
                    player.EnableEffect<Hemorrhage>(duration);
                    player.ShowHint($"You've been hemorrhaged for {duration} seconds");

                    break;
                case "mutate":
                    Role cachedMutatorRole = player.Role;

                    player.DropItems();
                    player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass);

                    Vector3 cachedPos = player.Position;

                    Timing.CallDelayed(duration, () => 
                    {
                        Log.Info(cachedMutatorRole.Type);

                        player.Role.Set(player.Role, SpawnReason.ForceClass);

                        player.Position = cachedPos;
                    });

                    player.ShowHint($"You've been mutated for {duration} seconds");

                    break;
                case "paper":
                    player.Scale = new(1f, 1f, 0.01f);
                    Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));
                    player.ShowHint($"You've been turned into paper for {duration} seconds");

                    break;
                case "sinkhole":
                    player.EnableEffect<Sinkhole>(duration);
                    player.ShowHint($"You've been given sinkhole effect for {duration} seconds");

                    break;
                case "scp268":
                    player.EnableEffect<Invisible>(duration);

                    player.ShowHint($"You've been turned invisible for {duration} seconds");

                    break;
                case "upsidedown":
                    player.Scale = new(1f, -1f, 1f);
                    Timing.CallDelayed(duration, () =>
                    {
                        player.Scale = new(1f, 1f, 1f);
                        player.Position += new Vector3(0, 1, 0);
                    });

                    player.ShowHint($"You've been converted to Australian for {duration} seconds");

                    break;
                case "sizedown":
                    player.Scale = new(0.5f, 0.5f, 0.5f);
                    Timing.CallDelayed(duration, () => player.Scale = new(1f, 1f, 1f));

                    player.ShowHint($"You've been cut in half for {duration} seconds");
                    
                    break;
                case "tpscp":
                    if (Player.Get(Side.Scp).Any())
                    {
                        Player scpPlayer = Player.Get(Side.Scp).ToList().RandomItem();
                        player.Position = scpPlayer.Position;
                        player.ShowHint($"You've been teleported to an SCP.");
                    }
                    else
                    {
                        player.ShowHint("No SCP found to teleport to.");
                    }
                    break;
                case "sonic":
                    player.EnableEffect<MovementBoost>(duration);
                    player.ChangeEffectIntensity<MovementBoost>(config.MovementBoostIntensity, duration);

                    player.ShowHint($"You've been made in to Sonic for {duration} seconds");
                    break;
                case "basketballplayer":
                    player.Scale = new Vector3(1.5f, 1.5f, 1.5f);

                    Timing.CallDelayed(duration, () => player.Scale = new Vector3(1f, 1f, 1f));

                    player.ShowHint($"You've been converted to a basketball player for {duration} seconds");

                    break;
                case "deafened":
                    player.EnableEffect<Deafened>(duration);

                    player.ShowHint($"You've been Deafened for {duration} seconds");
                    break;
                case "blinded":
                    player.EnableEffect<Blinded>(duration);

                    player.ShowHint($"You've been Blinded for {duration} seconds");
                    break;
                case "rndtp":
                    Room rand = Room.List.ElementAt(Random.Range(0, Room.List.Count()));
                    player.Position = rand.Position + Vector3.up;

                    player.ShowHint("You've been teleported to a random room.");
                    break;
                case "tptoply":
                    {
                        List<Player> players = Player.List.Where(p => p.IsScp == false && p != player).ToList();
                        if (players.Count() == 0)
                        {
                            player.ShowHint("No Player found to teleport.");
                        }
                        else
                        {
                            Player ply = players.ElementAt(Random.Range(0, players.Count()));
                            player.Position = ply.Position + Vector3.up;
                            player.ShowHint($"You've been teleported to {ply.Nickname}");
                        }
                        break;
                    }
                case "dropitems":
                    player.DropItems();

                    player.ShowHint("It looks like you dropped something.");
                    break;
                case "tantrum":
                    player.PlaceTantrum();

                    player.ShowHint($"It looks like you didn't get to the toilet in time.");
                    break;
                default:
                    player.ShowHint($"You've been {effectType} for {duration} seconds");
                    break;
            }
        }

        private string NextEffect() => plugin.Config.PossibleEffects[Random.Range(0, config.PossibleEffects.Count)];
    }
}
