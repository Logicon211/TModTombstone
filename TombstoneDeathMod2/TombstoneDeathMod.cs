using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TombstoneDeathMod2
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

        public void Send(int toWho, int fromWho, int x, int y)
        {
            ModPacket packet = GetPacket();
            if (Main.netMode == NetmodeID.Server)
            {
                packet.Write(fromWho);
            }
            packet.Write(x);
            packet.Write(y);
            packet.Send(toWho, fromWho);
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                fromWho = reader.ReadInt32();
            }

            int x = reader.ReadInt32();
            int y = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                Send(-1, fromWho, x ,y );
                //NetworkText text = NetworkText.FromFormattable("Server removing gravestone for {0}", Main.player[fromWho].name);
                //NetMessage.BroadcastChatMessage(text, Color.White);
                WorldGen.KillTile(x, y);
            }
            else
            {
                //Main.NewText("Remote player removing tombstone for " + Main.player[fromWho].name, 255, 100, 100);
                WorldGen.KillTile(x, y);
            }
        }
    }
}
