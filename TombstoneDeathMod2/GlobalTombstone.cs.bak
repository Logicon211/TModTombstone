using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.Localization;

namespace TombstoneDeathMod2
{
    public class GlobalTombstone : GlobalTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[TileID.Tombstones] = true;
        }

        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            //Makes the tombstones give off a bit of light
            if (type == TileID.Tombstones)
            {
                r = 1f;
                g = 1f;
                b = 1f;
            }
        }

        //TODO network sync removed gravestone
        public override void RightClick(int i, int j, int type)
        {
            if (type == TileID.Tombstones) {
                //Get player id and give him his shit bacK

                Player player = Main.player[Main.myPlayer];

                //mod.Logger.Warn("Player ID: " + Main.myPlayer + " " + player.name + " Tried to click on tombstone coordinates: " + i + ", " + j);

                //ASUMING i and j are x and y coordinates I can do some stuff
                //Need to check each tile at + and - 1 because the tile is 2x2, but the projectile will only save a single tile coordinate
                Point[] tombStonePositions = new Point[9];
                tombStonePositions[0] = new Point(i, j);
                tombStonePositions[1] = new Point(i, j+1);
                tombStonePositions[2] = new Point(i, j-1);
                tombStonePositions[3] = new Point(i+1, j);
                tombStonePositions[4] = new Point(i+1, j+1);
                tombStonePositions[5] = new Point(i+1, j-1);
                tombStonePositions[6] = new Point(i-1, j);
                tombStonePositions[7] = new Point(i-1, j+1);
                tombStonePositions[8] = new Point(i-1, j-1);

                TombstonePlayer tStonePlayer = player.GetModPlayer<TombstonePlayer>();

                Dictionary<Point, PlayerDeathInventory> playerDeathInventoryMap = tStonePlayer.playerDeathInventoryMap;
                PlayerDeathInventory playerDeathInventory = null;

                //loop through the 9 tile list
                for(int pos = 0; pos < tombStonePositions.Length; pos++) {
                    if (playerDeathInventoryMap.TryGetValue(tombStonePositions[pos], out playerDeathInventory))
                    {
                        //found the player's death inventory, give it to the player who clicked on it
                        //Player player = Main.player[playerId];

                        //First cause player to drop all items carried, then apply the items straight to his inventory
                        player.DropItems();

                        //INVENTORY
                        for (int c = 0; c < playerDeathInventory.deathInventory.Length; c++)
                        {
                            //put inventory into separate list
                            player.inventory[c] = playerDeathInventory.deathInventory[c];
                        }

                        //ARMOR - SOCIAL
                        for (int c = 0; c < playerDeathInventory.deathArmor.Length; c++)
                        {
                            //put armor into separate list
                            player.armor[c] = playerDeathInventory.deathArmor[c];
                        }

                        //DYES
                        for (int c = 0; c < playerDeathInventory.deathDye.Length; c++)
                        {
                            //put dye into separate list
                            player.dye[c] = playerDeathInventory.deathDye[c];
                        }

                        //EQUIPMENT
                        for (int c = 0; c < playerDeathInventory.deathMiscEquips.Length; c++)
                        {
                            //put equipment into separate list
                            player.miscEquips[c] = playerDeathInventory.deathMiscEquips[c];
                        }

                        //EQUIPMENT - DYE
                        for (int c = 0; c < playerDeathInventory.deathMiscDyes.Length; c++)
                        {
                            //put equipment dye into separate list
                            player.miscDyes[c] = playerDeathInventory.deathMiscDyes[c];
                        }

                        //delete existing player inventory at that spot
                        playerDeathInventoryMap.Remove(tombStonePositions[pos]);

                        WorldGen.KillTile(tombStonePositions[pos].X, tombStonePositions[pos].Y);

                        if (Main.netMode != NetmodeID.SinglePlayer) {
                            //TombstoneDeathMod myMod = (TombstoneDeathMod) mod;
                            //myMod.Send(-1, Main.myPlayer, tombStonePositions[pos].X, tombStonePositions[pos].Y);
                            Send(-1, Main.myPlayer, tombStonePositions[pos].X, tombStonePositions[pos].Y);
                        }
                        
                        break;
                    }
                }
            }
        }

        public void Send(int toWho, int fromWho, int x, int y)
        {
            //Main.NewText("Local player sending remove tombstone", 255, 100, 100);

            ModPacket packet = mod.GetPacket();
            if (Main.netMode == NetmodeID.Server)
            {
                packet.Write(fromWho);
            }
            packet.Write(x);
            packet.Write(y);
            packet.Send(toWho, fromWho);
        }
    }
}
