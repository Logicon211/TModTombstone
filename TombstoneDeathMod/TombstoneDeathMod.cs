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
            AddPlayer("Tombstone Player", new TombstonePlayer());
            AddGlobalTile("Global Tombstone", new GlobalTombstone());
            AddGlobalProjectile("Tombstone Projectile", new TombstoneGlobalProjectile());
        }
    }
}
