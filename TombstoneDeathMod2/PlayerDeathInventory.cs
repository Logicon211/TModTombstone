using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TombstoneDeathMod2
{
    public class PlayerDeathInventory
    {
        public Item[] deathInventory;
        public Item[] deathArmor;
        public Item[] deathDye;
        public Item[] deathMiscEquips;
        public Item[] deathMiscDyes;

        public PlayerDeathInventory(Item[] dInventory, Item[] dArmor, Item[] dDye, Item[] dMiscEquips, Item[] dMiscDye)
        {
            deathInventory = dInventory;
            deathArmor = dArmor;
            deathDye = dDye;
            deathMiscEquips = dMiscEquips;
            deathMiscDyes = dMiscDye;
        }

        public int getValue()
        {
            int value = 0;

            for (int i = 0; i < deathInventory.Length; i++)
            {
                value += deathInventory[i].value;
            }

            for (int i = 0; i < deathArmor.Length; i++)
            {
                value += deathArmor[i].value;
            }

            for (int i = 0; i < deathMiscEquips.Length; i++)
            {
                value += deathMiscEquips[i].value;
            }

            return value;
        }
    }
}
