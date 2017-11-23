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

namespace TombstoneDeathMod
{
    public class GlobalTombstone : GlobalTile
    {
        public static Dictionary<string, PlayerDeathInventory> playerDeathInventoryMap = new Dictionary<string, PlayerDeathInventory>();

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

        public override void RightClick(int i, int j, int type)
        {
            if (type == TileID.Tombstones) {
                //Get player id and give him his shit bacK

                //Maybe get closest player?
                Player[] players = Main.player;

                Player player = Main.player[Main.myPlayer];

                Debug.WriteLine("Player ID: " + Main.myPlayer + " Tried to click on tombstone coordinates: " + i + ", " + j);
                Debug.WriteLine("Player's Name was " + Main.player[Main.myPlayer].name);

                //Don't need this as apparently we can get myMainPlayer here
                /*Player closestPlayer = null;
                float closestPlayerDistance = 0f;
                foreach (Player player in players)
                {
                    if (closestPlayer == null)
                    {
                        closestPlayer = player;
                        closestPlayerDistance = Vector2.Distance(new Vector2(closestPlayer.position.X / 16, closestPlayer.position.Y / 16), new Vector2(i, j));
                    } else
                    {
                        Vector2 comparingPlayerPosition = new Vector2(player.position.X / 16, player.position.Y / 16);

                        float playerDistance = Vector2.Distance(comparingPlayerPosition, new Vector2(i, j));

                        if(playerDistance < closestPlayerDistance)
                        {
                            closestPlayerDistance = playerDistance;
                            closestPlayer = player;
                        }
                    }
                }*/

                //ASUMING i and j are x and y coordinates I can do some stuff
                //Need to check each tile at + and - 1 because the tile is 2x2, but the projectile will only save a single tile coordinate
                Vector2[] tombStonePositions = new Vector2[9];
                tombStonePositions[0] = new Vector2(i, j);
                tombStonePositions[1] = new Vector2(i, j+1);
                tombStonePositions[2] = new Vector2(i, j-1);
                tombStonePositions[3] = new Vector2(i+1, j);
                tombStonePositions[4] = new Vector2(i+1, j+1);
                tombStonePositions[5] = new Vector2(i+1, j-1);
                tombStonePositions[6] = new Vector2(i-1, j);
                tombStonePositions[7] = new Vector2(i-1, j+1);
                tombStonePositions[8] = new Vector2(i-1, j-1);

                //NEED TO GET THIS SOMEHOW OH FUCK GOD
                string playerName = player.name;

                PlayerDeathInventory playerDeathInventory = null;

                //loop through the 9 tile list
                for(int pos = 0; pos < tombStonePositions.Length; pos++) {
                    if (playerDeathInventoryMap.TryGetValue(playerName + "," + tombStonePositions[pos].ToString(), out playerDeathInventory))
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
                        playerDeathInventoryMap.Remove(playerName + "," + tombStonePositions[pos].ToString());
                        WorldGen.KillTile((int)tombStonePositions[pos].X, (int)tombStonePositions[pos].Y);
                        break;

                    }
                    else
                    {
                        //Didn't find it, do nothing?
                    }
                }
            }
        }
    }
}
