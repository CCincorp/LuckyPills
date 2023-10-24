using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace LuckyPills
{
    public class Translation : ITranslation
    {

        [Description("Set a different hint message. NOTE: {0} will be replaced with the seconds or player's name depending on the effect")]
        public Dictionary<string, string> EffectHints { get; set; } = new()
        {
            {"bombvomit",  "You started vomiting Grenades for {0} seconds"},
            {"ballvomit",  "You started vomiting Balls for {0} seconds"},
            {"grenade",  "Run, Grenade"},
            {"flashed",  "Watch out Flashbang"},
            {"flashvomit",  "You started vomiting Flashbangs for {0} seconds"},
            {"god",  "You received god mode for {0} seconds"},
            {"mutate",  "You mutated in to a Zombie for {0} seconds"},
            {"paper",  "You got turned into paper for {0} seconds"},
            {"upsidedown",  "You are Australian for {0} seconds"},
            {"sizedown",  "You shrunk in size for {0} seconds"},
            {"tpscp",  "Say Hello to {0}"},
            {"tpscpfailed",  "No SCP found to teleport to."},
            {"sonic",  "You are Sonic for {0} seconds"},
            {"basketballplayer",  "Lebron James for {0} seconds"},
            {"rndtp",  "You've been teleported to a random room."},
            {"rndtpwarhead", "Warhead was Detonated."},
            {"tptoplyfailed",  "No Player found to teleport."},
            {"tptoply",  "You've been teleported to {0}"},
            {"dropitems",  "Your pockets have holes"},
            {"tantrum",  "Shitted all over the floor"},
            {"badeffect", "You received {0} effect for {1} seconds"},
            {"goodeffect", "You received {0} effect for {1} seconds"},
            {"increasehealth", "Your health has increased"},
            {"grandma", "You dropped your Grandma :("},
            {"randomitem", "You got a {0}"},
            {"randomteam", "You are now a {0}"},
            {"flipnukeswitchON", "You flipped the Nuke Lever on"},
            {"flipnukeswitchOFF", "You flipped the Nuke Lever off"},
            {"bypass", "You can open all doors for {0} seconds"},
            {"ap", "Your Shield has increased by 100 AP"},
            {"primitive",  "Ball"},
            {"pinkcandy", "On the ground there lies a mysteries Pink Candy"},
            {"UnknownEffect", "Unknown Effect: {0}"}
        };
    }
}
