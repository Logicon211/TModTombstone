using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TombstoneDeathMod2
{
    class TombstoneGlobalProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.Tombstone:
                case ProjectileID.GraveMarker:
                case ProjectileID.CrossGraveMarker:
                case ProjectileID.Headstone:
                case ProjectileID.Gravestone:
                case ProjectileID.Obelisk:
                case ProjectileID.RichGravestone1:
                case ProjectileID.RichGravestone2:
                case ProjectileID.RichGravestone3:
                case ProjectileID.RichGravestone4:
                case ProjectileID.RichGravestone5:
                    projectile.active = false;
                    return;
            }
        }
    }
}
