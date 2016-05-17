using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TombstoneDeathMod
{
    public class TombstonePlayer : ModPlayer
    {

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref string deathText)
        {
            //deathText = " was dissolved by holy powers";
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, string deathText)
        {
            deathText = " More god damned death text";
        }

    }
}
