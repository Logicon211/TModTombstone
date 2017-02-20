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
        public PlayerDeathInventory playerDeathInventory = null;

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //deathText = " was dissolved by holy powers";

            Item[] deathInventory = new Item[player.inventory.Length];
            Item[] deathArmor = new Item[player.armor.Length];
            Item[] deathDye = new Item[player.dye.Length];
            Item[] deathMiscEquips = new Item[player.miscEquips.Length];
            Item[] deathMiscDyes = new Item[player.miscDyes.Length];

            //INVENTORY
            for (int i = 0; i < player.inventory.Length; i++)
            {
                //put inventory into separate list
                deathInventory[i] = player.inventory[i];
                player.inventory[i] = new Item();
            }

            //ARMOR - SOCIAL
            for (int i = 0; i < player.armor.Length; i++)
            {
                //put armor into separate list
                deathArmor[i] = player.armor[i];
                player.armor[i] = new Item();
            }

            //DYES
            for (int i = 0; i < player.dye.Length; i++)
            {
                //put dye into separate list
                deathDye[i] = player.dye[i];
                player.dye[i] = new Item();
            }

            //EQUIPMENT
            for (int i = 0; i < player.miscEquips.Length; i++)
            {
                //put equipment into separate list
                deathMiscEquips[i] = player.miscEquips[i];
                player.miscEquips[i] = new Item();
            }

            //EQUIPMENT - DYE
            for (int i = 0; i < player.miscDyes.Length; i++)
            {
                //put equipment dye into separate list
                deathMiscDyes[i] = player.miscDyes[i];
                player.miscDyes[i] = new Item();
            }

            playerDeathInventory = new PlayerDeathInventory(deathInventory, deathArmor, deathDye, deathMiscEquips, deathMiscDyes);
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            //Dunno what I can do here. I can't change death text anymore?
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

    }
}
