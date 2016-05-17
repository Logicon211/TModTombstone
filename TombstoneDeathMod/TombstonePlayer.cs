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
            
            //INVENTORY
            for (int i = 0; i < player.inventory.Length; i++)
            {
                //put inventory into separate list
                player.inventory[i] = new Item();
            }

            //ARMOR - SOCIAL
            for (int i = 0; i < player.armor.Length; i++)
            {
                //put armor into separate list
                player.armor[i] = new Item();
            }

            //DYES
            for (int i = 0; i < player.dye.Length; i++)
            {
                //put dye into separate list
                player.dye[i] = new Item();
            }

            //EQUIPMENT
            for (int i = 0; i < player.miscEquips.Length; i++)
            {
                //put equipment into separate list
                player.miscEquips[i] = new Item();
            }

            //EQUIPMENT - DYE
            for (int i = 0; i < player.miscDyes.Length; i++)
            {
                //put equipment dye into separate list
                player.miscEquips[i] = new Item();
            }

            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, string deathText)
        {
            deathText = " More god damned death text";
        }

    }
}
