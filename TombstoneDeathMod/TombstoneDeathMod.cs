using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TombstoneDeathMod
{
    public class TombstoneDeathMod: Terraria.ModLoader.Mod
    {
        public override void Load()
        {
            //Tmodloader has been updated so that I probably don't need to do the below anymore (In fact it probably breaks stuff)

            //AddPlayer("Tombstone Player", new TombstonePlayer());
            //AddGlobalTile("Global Tombstone", new GlobalTombstone());
            //AddGlobalProjectile("Tombstone Projectile", new TombstoneGlobalProjectile());
        }
    }
}
