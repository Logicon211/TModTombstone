﻿using Microsoft.Xna.Framework;
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

namespace TombstoneDeathMod
{
    class TombstoneGlobalProjectile : GlobalProjectile
    {


        /*public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == ProjectileID.Obelisk  || projectile.type == ProjectileID.GraveMarker || projectile.type == ProjectileID.Gravestone || projectile.type == ProjectileID.Tombstone || projectile.type == ProjectileID.Headstone || projectile.type == ProjectileID.CrossGraveMarker || projectile.type == ProjectileID.RichGravestone1 || projectile.type == ProjectileID.RichGravestone2 || projectile.type == ProjectileID.RichGravestone3 || projectile.type == ProjectileID.RichGravestone4 || projectile.type == ProjectileID.RichGravestone5)
            {
               string test = projectile.ToString();
                GlobalTombstone gt = (GlobalTombstone)mod.GetGlobalTile("Global Tombstone");

                //Figure out who owns the projectile. Then get that player's "Death" storage and put it in GlobalTombstone's map storing player's inventory by tombston location

                //Not 100% sure what owner does, can I get a list of players?
                int owner = projectile.owner;

                Vector2 tombStonePosition = new Vector2((int) Math.Ceiling(projectile.position.X/16), (int) Math.Ceiling(projectile.position.Y/16));
                
                //Debug.WriteLine("Player Died at " + tombStonePosition.ToString() + " player ID: " + projectile.owner);
                //Debug.WriteLine("Player's Name was " + Main.player[Main.myPlayer].name);

                TombstonePlayer player = ((TombstonePlayer)Main.player[owner].GetModPlayer(mod, "Tombstone Player"));
                PlayerDeathInventory playerDeathInventory = player.playerDeathInventory;

                Dictionary<string, PlayerDeathInventory> playerDeathInventoryMap = ((GlobalTombstone) mod.GetGlobalTile("Global Tombstone")).playerDeathInventoryMap;

                if(playerDeathInventoryMap.ContainsKey(Main.player[owner].name + "," + tombStonePosition.ToString()))
                {
                    //remove inventory if one for some reason exists already for that player there Could only happen if a tombstone was removed there)
                    playerDeathInventoryMap.Remove(Main.player[owner].name + "," + tombStonePosition.ToString());
                }
                playerDeathInventoryMap.Add(Main.player[owner].name + "," + tombStonePosition.ToString(), playerDeathInventory);

                //clear playerDeathInventory (Might be a bad idea?)
                player.playerDeathInventory = null;
            }
            return true;
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == ProjectileID.Obelisk || projectile.type == ProjectileID.GraveMarker || projectile.type == ProjectileID.Gravestone || projectile.type == ProjectileID.Tombstone || projectile.type == ProjectileID.Headstone || projectile.type == ProjectileID.CrossGraveMarker || projectile.type == ProjectileID.RichGravestone1 || projectile.type == ProjectileID.RichGravestone2 || projectile.type == ProjectileID.RichGravestone3 || projectile.type == ProjectileID.RichGravestone4 || projectile.type == ProjectileID.RichGravestone5)
            {
                //lastCollisionPosition = oldVelocity;

                int x = (int)((projectile.position.X) / 16f);
                int y = (int)((projectile.position.Y) / 16f);

                WorldGen.KillTile(x, y);
                WorldGen.KillTile(x, y + 1);
                WorldGen.KillTile(x, y - 1);
                WorldGen.KillTile(x + 1, y);
                WorldGen.KillTile(x + 1, y + 1);
                WorldGen.KillTile(x + 1, y - 1);
                WorldGen.KillTile(x - 1, y);
                WorldGen.KillTile(x - 1, y + 1);
                WorldGen.KillTile(x - 1, y - 1);

                Main.tile[x, y + 1].active(true);
                Main.tile[x + 1, y + 1].active(true);
                Main.tile[x - 1, y + 1].active(true);

                WorldGen.PlaceTile(x, y, TileID.Tombstones, false, true, projectile.owner, 5);

                //Figure out who owns the projectile. Then get that player's "Death" storage and put it in GlobalTombstone's map storing player's inventory by tombston location

                //Not 100% sure what owner does, can I get a list of players?
                int owner = projectile.owner;

                Vector2 tombStonePosition = new Vector2(x, y);

                //Debug.WriteLine("Player Died at " + tombStonePosition.ToString() + " player ID: " + projectile.owner);
                //Debug.WriteLine("Player's Name was " + Main.player[Main.myPlayer].name);

                TombstonePlayer player = ((TombstonePlayer)Main.player[owner].GetModPlayer(mod, "Tombstone Player"));
                PlayerDeathInventory playerDeathInventory = player.playerDeathInventory;

                Dictionary<string, PlayerDeathInventory> playerDeathInventoryMap = ((GlobalTombstone)mod.GetGlobalTile("Global Tombstone")).playerDeathInventoryMap;

                if (playerDeathInventoryMap.ContainsKey(Main.player[owner].name + "," + tombStonePosition.ToString()))
                {
                    //remove inventory if one for some reason exists already for that player there Could only happen if a tombstone was removed there)
                    playerDeathInventoryMap.Remove(Main.player[owner].name + "," + tombStonePosition.ToString());
                }
                playerDeathInventoryMap.Add(Main.player[owner].name + "," + tombStonePosition.ToString(), playerDeathInventory);

                //clear playerDeathInventory (Might be a bad idea?)
                player.playerDeathInventory = null;

                projectile.Kill();
            }
            return true;
        }*/
    }
}
