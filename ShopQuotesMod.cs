using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShopQuotesMod
{
    public partial class ShopQuotesMod : Mod
    {
        public static ShopQuotesMod Instance { get; private set; }

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            AutomaticLocalizationPaths.Clear();
            Instance = null;
        }

        public static string GetItemKeyName(int itemID, Mod myMod = null)
        {
            if (itemID < Main.maxItemTypes)
                return ItemID.Search.GetName(itemID);

            var modItem = ItemLoader.GetItem(itemID);
            if (myMod != null && modItem.Mod.Name == myMod.Name)
                return modItem.Name;

            return $"{modItem.Mod.Name}_{modItem.Name}";
        }

        public static string GetNPCKeyName(int npcID, Mod myMod = null)
        {
            if (npcID < Main.maxNPCTypes)
                return NPCID.Search.GetName(npcID);

            var modNPC = NPCLoader.GetNPC(npcID);
            if (myMod != null && modNPC.Mod.Name == myMod.Name)
                return modNPC.Name;

            return $"{modNPC.Mod.Name}_{modNPC.Name}";
        }

        #region Helpers
        public static Color MaxRGBA(Color color, byte amt)
        {
            return MaxRGBA(color, amt, amt);
        }
        public static Color MaxRGBA(Color color, byte amt, byte a)
        {
            return MaxRGBA(color, amt, amt, amt, a);
        }
        public static Color MaxRGBA(Color color, byte r, byte g, byte b, byte a)
        {
            color.R = Math.Max(color.R, r);
            color.G = Math.Max(color.G, g);
            color.B = Math.Max(color.B, b);
            color.A = Math.Max(color.A, a);
            return color;
        }

        public static Color LerpBetween(Color[] colors, float amount)
        {
            if (amount < 0f)
            {
                amount %= colors.Length;
                amount = colors.Length - amount;
            }
            int index = (int)amount;
            return Color.Lerp(colors[index % colors.Length], colors[(index + 1) % colors.Length], amount % 1f);
        }
        #endregion
    }
}