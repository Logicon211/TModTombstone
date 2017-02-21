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
        //public PlayerDeathInventory playerDeathInventory = null;

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

            PlayerDeathInventory playerDeathInventory = new PlayerDeathInventory(deathInventory, deathArmor, deathDye, deathMiscEquips, deathMiscDyes);

            
            int x = (int)((player.position.X) / 16f);
            int y = (int)((player.position.Y) / 16f) + 2;


            WorldGen.KillTile(x, y);
            WorldGen.KillTile(x, y+1);
            WorldGen.KillTile(x, y-1);
            WorldGen.KillTile(x+1, y);
            WorldGen.KillTile(x+1, y+1);
            WorldGen.KillTile(x+1, y-1);
            //WorldGen.KillTile(x-1, y);
            //WorldGen.KillTile(x-1, y+1);
            //WorldGen.KillTile(x-1, y-1);

            //TODO: Check for tile empty after killing the tiles to make sure there's space for the thing? Move it upwards if not?
            WorldGen.TileEmpty(x, y);

            Main.tile[x, y + 1].active(true);
            Main.tile[x + 1, y + 1].active(true);
           // Main.tile[x - 1, y + 1].active(true);
            
            WorldGen.PlaceTile(x, y, TileID.Tombstones, false, true, 1, 7);

            int sign = Sign.ReadSign(x, y, true);
            if (sign >= 0)
            {
                Sign.TextSign(sign, player.name + "'s Stuff");
            }

            Vector2 tombStonePosition = new Vector2(x, y);

            //Debug.WriteLine("Player Died at " + tombStonePosition.ToString() + " player ID: " + projectile.owner);
            //Debug.WriteLine("Player's Name was " + Main.player[Main.myPlayer].name);

            TombstonePlayer tStonePlayer = ((TombstonePlayer) player.GetModPlayer(mod, "Tombstone Player"));

            Dictionary<string, PlayerDeathInventory> playerDeathInventoryMap = ((GlobalTombstone)mod.GetGlobalTile("Global Tombstone")).playerDeathInventoryMap;

            if (playerDeathInventoryMap.ContainsKey(player.name + "," + tombStonePosition.ToString()))
            {
                //remove inventory if one for some reason exists already for that player there Could only happen if a tombstone was removed there)
                playerDeathInventoryMap.Remove(player.name + "," + tombStonePosition.ToString());
            }
            playerDeathInventoryMap.Add(player.name + "," + tombStonePosition.ToString(), playerDeathInventory);

            //clear playerDeathInventory (Might be a bad idea?)
            //tStonePlayer.playerDeathInventory = null;

            //*********************************************************************************

            //TODO: Figure out what the hell to do with Chests not being destroyable

            
            /* Main.tile[x, y].ClearEverything();
             Main.tile[x, y+1].ClearEverything();
             Main.tile[x, y-1].ClearEverything();
             Main.tile[x+1, y].ClearEverything();
             Main.tile[x+1, y+1].ClearEverything();
             Main.tile[x+1, y-1].ClearEverything();
             Main.tile[x-1, y].ClearEverything();
             Main.tile[x-1, y+1].ClearEverything();
             Main.tile[x-1, y-1].ClearEverything();*/


            /* WorldGen.PoundTile(x, y);
             WorldGen.PoundTile(x, y+1);
             WorldGen.PoundTile(x, y-1);
             WorldGen.PoundTile(x+1, y);
             WorldGen.PoundTile(x+1, y+1);
             WorldGen.PoundTile(x+1, y-1);
             WorldGen.PoundTile(x-1, y);
             WorldGen.PoundTile(x-1, y+1);
             WorldGen.PoundTile(x-1, y-1);
             WorldGen.TileEmpty(x, y);
             WorldGen.TileEmpty(x, y + 1);
             WorldGen.TileEmpty(x, y - 1);
             WorldGen.TileEmpty(x + 1, y);
             WorldGen.TileEmpty(x + 1, y + 1);
             WorldGen.TileEmpty(x + 1, y - 1);
             WorldGen.TileEmpty(x - 1, y);
             WorldGen.TileEmpty(x - 1, y + 1);
             WorldGen.TileEmpty(x - 1, y - 1);
             Main.*/
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            //Dunno what I can do here. I can't change death text anymore?
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

    }
}
