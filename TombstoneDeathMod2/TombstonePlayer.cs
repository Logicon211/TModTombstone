using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TombstoneDeathMod2
{
    public class TombstonePlayer : ModPlayer
    {
        public Dictionary<Point, PlayerDeathInventory> playerDeathInventoryMap;
        int loadedValue;

        public override void Initialize()
        {
            playerDeathInventoryMap = new Dictionary<Point, PlayerDeathInventory>();
        }

        // TT 24
        // CT 13
        // GG 56
        private Boolean isTileCoordinateClear(int x, int y)
        {
            Tile t1 = Main.tile[x, y];
            Tile t2 = Main.tile[x, y - 1];
            Tile t3 = Main.tile[x + 1, y];
            Tile t4 = Main.tile[x + 1 , y - 1];
            Tile t5 = Main.tile[x, y + 1];
            Tile t6 = Main.tile[x + 1, y + 1];

            Boolean belowClearOrGround = (WorldGen.TileEmpty(x, y + 1) || t5.HasTile && Main.tileSolid[t5.TileType] && !t5.IsHalfBlock && t5.Slope == 0 && !Main.tileNoAttach[t5.TileType])
                                      && (WorldGen.TileEmpty(x + 1, y + 1) || t6.HasTile && Main.tileSolid[t6.TileType] && !t6.IsHalfBlock && t6.Slope == 0 && !Main.tileNoAttach[t6.TileType]);

            Boolean clear = WorldGen.TileEmpty(x, y) && WorldGen.TileEmpty(x, y - 1) && WorldGen.TileEmpty(x + 1, y) && WorldGen.TileEmpty(x + 1, y - 1)
                && belowClearOrGround;

            Boolean mostlyClear = (WorldGen.TileEmpty(x, y) || (t1.HasTile && Main.tileCut[t1.TileType]))
                && (WorldGen.TileEmpty(x, y - 1) || (t2.HasTile && Main.tileCut[t2.TileType]))
                && (WorldGen.TileEmpty(x + 1, y) || (t3.HasTile && Main.tileCut[t3.TileType]))
                && (WorldGen.TileEmpty(x + 1, y - 1) || (t4.HasTile && Main.tileCut[t4.TileType]))
                && belowClearOrGround;

            //Main.NewText("test X " + x + ", Y " + y + ", clear=" + clear + ", mostlyClear=" + mostlyClear);

            if (mostlyClear)
            {
                // Remove grass
                if (Main.tile[x, y].HasTile)
                {
                    WorldGen.KillTile(x, y, false, false, false);
                }
                if (Main.tile[x, y - 1].HasTile)
                {
                    WorldGen.KillTile(x, y - 1, false, false, false);
                }
                if (Main.tile[x + 1, y].HasTile)
                {
                    WorldGen.KillTile(x + 1, y, false, false, false);
                }
                if (Main.tile[x + 1, y - 1].HasTile)
                {
                    WorldGen.KillTile(x + 1, y - 1, false, false, false);
                }
            }

            if (clear || mostlyClear)
            {
                // Create dirt below
                if (WorldGen.TileEmpty(x, y + 1))
                {
                    Tile tile = Main.tile[x, y + 1];
                    tile.HasTile = true;
                }
                if (WorldGen.TileEmpty(x + 1, y + 1))
                {
                    Tile tile = Main.tile[x + 1, y + 1];
                    tile.HasTile = true;
                }

                return true;
            }

            return false;
        }

        //public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        //{

        //}
       // {
        //    return true;
        //}


        //public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            Point playerPosition = Player.position.ToTileCoordinates();
            int x = playerPosition.X;
            int y = playerPosition.Y + 4;
            int startX = x;

            bool isClearForTombstone = isTileCoordinateClear(x, y);

            // Check left and right 15 squares, then up one and repeat 15 times
            int movesY = 0;
            while (!isClearForTombstone && movesY++ < 15)
            {

                x = startX;
                isClearForTombstone = isTileCoordinateClear(x, y);

                int movesX = 0;
                while (!isClearForTombstone && movesX++ < 15)
                {
                    x = startX - movesX;

                    if (x < 0)
                    {
                        x = 0;
                    }

                    isClearForTombstone = isTileCoordinateClear(x, y);

                    if (!isClearForTombstone)
                    {
                        x = startX + movesX;

                        if (x > Main.maxTilesX)
                        {
                            x = Main.maxTilesX - 1;
                        }

                        isClearForTombstone = isTileCoordinateClear(x, y);
                    }
                }

                if (!isClearForTombstone)
                {
                    y--;

                    if (y < 0)
                    {
                        y = 0;
                    }
                }
            }

            if (!isClearForTombstone)
            {
                // Revert to normal death
                Main.NewText("Unable to place tombstone1. Reverting to normal death.", 255, 100, 100);
                base.Kill(damage, hitDirection, pvp, damageSource);
                return true;
            }

            if (!WorldGen.PlaceTile(x, y, TileID.Tombstones, false, true, 1, 7))
            {
                // Revert to normal death
                Main.NewText("Unable to place tombstone2. Reverting to normal death.", 255, 100, 100);
                base.Kill(damage, hitDirection, pvp, damageSource);
                return true;
            }

            int sign = Sign.ReadSign(x, y, true);
            if (sign >= 0)
            {
                Sign.TextSign(sign, Player.name + "'s Stuff");
            }

            Point tombStonePosition = new Point(x, y);

            PlayerDeathInventory previousInventory = null;

            if (playerDeathInventoryMap.TryGetValue(tombStonePosition, out previousInventory))
            {
                int oldItemValue = previousInventory.getValue();

                int newItemValue = 0;

                for (int i = 0; i < Player.inventory.Length; i++)
                {
                    newItemValue += Player.inventory[i].value;
                }

                for (int i = 0; i < Player.armor.Length; i++)
                {
                    newItemValue += Player.armor[i].value;
                }

                for (int i = 0; i < Player.miscEquips.Length; i++)
                {
                    newItemValue += Player.miscEquips[i].value;
                }

                // Remove previous inventory only if new death was more valuable
                if (newItemValue < oldItemValue)
                {
                    Main.NewText("Previous more valuable death at same position, not overwriting. Reverting to normal death.", 255, 100, 100);
                    base.Kill(damage, hitDirection, pvp, damageSource);
                    return true;
                }

                Main.NewText("Previous less valuable death at same position, overwriting.", 255, 100, 100);

                playerDeathInventoryMap.Remove(tombStonePosition);
            }

            Item[] deathInventory = new Item[Player.inventory.Length];
            Item[] deathArmor = new Item[Player.armor.Length];
            Item[] deathDye = new Item[Player.dye.Length];
            Item[] deathMiscEquips = new Item[Player.miscEquips.Length];
            Item[] deathMiscDyes = new Item[Player.miscDyes.Length];

            //INVENTORY
            for (int i = 0; i < Player.inventory.Length; i++)
            {
                //put inventory into separate list
                deathInventory[i] = Player.inventory[i];
                Player.inventory[i] = new Item();
            }

            //ARMOR - SOCIAL
            for (int i = 0; i < Player.armor.Length; i++)
            {
                //put armor into separate list
                deathArmor[i] = Player.armor[i];
                Player.armor[i] = new Item();
            }

            //DYES
            for (int i = 0; i < Player.dye.Length; i++)
            {
                //put dye into separate list
                deathDye[i] = Player.dye[i];
                Player.dye[i] = new Item();
            }

            //EQUIPMENT
            for (int i = 0; i < Player.miscEquips.Length; i++)
            {
                //put equipment into separate list
                deathMiscEquips[i] = Player.miscEquips[i];
                Player.miscEquips[i] = new Item();
            }

            //EQUIPMENT - DYE
            for (int i = 0; i < Player.miscDyes.Length; i++)
            {
                //put equipment dye into separate list
                deathMiscDyes[i] = Player.miscDyes[i];
                Player.miscDyes[i] = new Item();
            }

            PlayerDeathInventory playerDeathInventory = new PlayerDeathInventory(deathInventory, deathArmor, deathDye, deathMiscEquips, deathMiscDyes);

            playerDeathInventoryMap.Add(tombStonePosition, playerDeathInventory);

            Main.NewText("Inventory saved in tombstone at " + pointToText(tombStonePosition));

            return true;
            //base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public override void SaveData(TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */
        {
            int maxValue = 0;
            Point position = new Point();
            PlayerDeathInventory mostValuableDeath = null;

            foreach (KeyValuePair<Point, PlayerDeathInventory> entry in playerDeathInventoryMap)
            {
                int value = entry.Value.getValue();
                if (value > maxValue)
                {
                    maxValue = value;
                    position.X = entry.Key.X;
                    position.Y = entry.Key.Y;
                    mostValuableDeath = entry.Value;
                }
            }

            if (mostValuableDeath == null)
            {
                return;
            }

            Mod.Logger.Info("Saving inventory in tombstone at " + position.X + ", " + position.Y + ", valued at " + maxValue);

            List<Item> deathInventory = new List<Item>(mostValuableDeath.deathInventory);
            List<Item> deathArmor = new List<Item>(mostValuableDeath.deathArmor);
            List<Item> deathDye = new List<Item>(mostValuableDeath.deathDye);
            List<Item> deathMiscEquips = new List<Item>(mostValuableDeath.deathMiscEquips);
            List<Item> deathMiscDyes = new List<Item>(mostValuableDeath.deathMiscDyes);

            nullEmptyItems(deathInventory);
            nullEmptyItems(deathArmor);
            nullEmptyItems(deathDye);
            nullEmptyItems(deathMiscEquips);
            nullEmptyItems(deathMiscDyes);

            //TagCompound tag = new TagCompound();
            tag.Add("x", position.X);
            tag.Add("y", position.Y);
            tag.Add("value", maxValue);
            tag.Add("deathInventory", deathInventory);
            tag.Add("deathArmor", deathArmor);
            tag.Add("deathDye", deathDye);
            tag.Add("deathMiscEquips", deathMiscEquips);
            tag.Add("deathMiscDyes", deathMiscDyes);
        }

        private void nullEmptyItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //mod.Logger.Warn("item type " + items[i].type + " name " + items[i].Name);
                   
                if (items[i].type == 0)
                {
                    //items[i] = null;
                }
            }
        }


        public override void LoadData(TagCompound tag)
        {
           try
            {
                int value = tag.GetInt("value");

                if (value > 0)
                {

                    loadedValue = value;

                    Point position = new Point(tag.GetInt("x"), tag.GetInt("y"));

                    Item[] deathInventory = new Item[Player.inventory.Length];
                    Item[] deathArmor = new Item[Player.armor.Length];
                    Item[] deathDye = new Item[Player.dye.Length];
                    Item[] deathMiscEquips = new Item[Player.miscEquips.Length];
                    Item[] deathMiscDyes = new Item[Player.miscDyes.Length];

                    loadItemList(tag.Get<List<Item>>("deathInventory"), deathInventory);
                    loadItemList(tag.Get<List<Item>>("deathArmor"), deathArmor);
                    loadItemList(tag.Get<List<Item>>("deathDye"), deathDye);
                    loadItemList(tag.Get<List<Item>>("deathMiscEquips"), deathMiscEquips);
                    loadItemList(tag.Get<List<Item>>("deathMiscDyes"), deathMiscDyes);

                    PlayerDeathInventory inventory = new PlayerDeathInventory(deathInventory, deathArmor, deathDye, deathMiscEquips, deathMiscDyes);

                    playerDeathInventoryMap.Add(position, inventory);
                }
            }
            catch (Exception e)
            {
                Mod.Logger.Error("Error loading saved tombstone inventory " + e.Message);
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (playerDeathInventoryMap.Count > 0)
            {
                foreach(KeyValuePair<Point, PlayerDeathInventory> entry in playerDeathInventoryMap)
                {
                    PlayerDeathInventory inventory = entry.Value;
                    Main.NewText("Loaded inventory in tombstone at " + pointToText(entry.Key) + ", valued " + Main.ValueToCoins(loadedValue), 155, 155, 255);
                }
                
            }
        }

        private void loadItemList(List<Item> items, Item[] inventory)
        {
            for (int i = 0; i < inventory.Length && i < items.Count; i++)
            {
                //mod.Logger.Warn("item type " + items[i].type + " name " + items[i].Name);

                if ("Unloaded Item".Equals(items[i].Name))
                {
                    inventory[i] = new Item();
                } else
                {
                    inventory[i] = items[i];
                }
            }
        }

        private string pointToText(Point point)
        {
            Vector2 worldCords = point.ToWorldCoordinates();

            string xText = "";
            int x = (int)((worldCords.X + (float)(Main.player[Main.myPlayer].width / 2)) * 2f / 16f - (float)Main.maxTilesX);

            if (x > 0)
            {
                xText += Language.GetTextValue("GameUI.CompassEast", x);
            }
            else if (x < 0)
            {
                xText += Language.GetTextValue("GameUI.CompassWest", -x);
            }
            else
            {
                xText += Language.GetTextValue("GameUI.CompassCenter");
            }

            int y = (int)((double)((worldCords.Y + (float)Main.player[Main.myPlayer].height) * 2f / 16f) - Main.worldSurface * 2.0);
            float num23 = (float)(Main.maxTilesX / 4200);
            num23 *= num23;
            int num24 = 1200;
            float num25 = (float)((double)((Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f - (65f + 10f * num23)) / (Main.worldSurface / 5.0));
            string layer;
            if (Main.player[Main.myPlayer].position.Y > (float)((Main.maxTilesY - 204) * 16))
            {
                layer = Language.GetTextValue("GameUI.LayerUnderworld");
            }
            else if ((double)worldCords.Y > Main.rockLayer * 16.0 + (double)(num24 / 2) + 16.0)
            {
                layer = Language.GetTextValue("GameUI.LayerCaverns");
            }
            else if (y > 0)
            {
                layer = Language.GetTextValue("GameUI.LayerUnderground");
            }
            else if (num25 >= 1f)
            {
                layer = Language.GetTextValue("GameUI.LayerSurface");
            }
            else
            {
                layer = Language.GetTextValue("GameUI.LayerSpace");
            }
            y = Math.Abs(y);
            string depth;
            if (y == 0)
            {
                depth = Language.GetTextValue("GameUI.DepthLevel");
            }
            else
            {
                depth = Language.GetTextValue("GameUI.Depth", y);
            }

            return xText + " " + depth + " " + layer;
        }

    }
}
