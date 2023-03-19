using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ShopQuotesMod
{
    public class QuoteData
    {
        public int NPCID { get; set; }
        public readonly string DefaultKey;
        public Func<QuoteData, int, int, Color> ColorMethod;
        public Func<Asset<Texture2D>> HeadTexture;

        public List<Func<int, string>> FindDefaultText;

        private readonly Dictionary<int, IQuoteProvider> Text;
        private readonly Mod Mod;

        internal QuoteData(Mod mod, int npcID, string defaultKey = "Mods.ShopQuotesMod.")
        {
            Mod = mod;
            DefaultKey = $"{defaultKey}{ShopQuotesMod.GetNPCKeyName(npcID, mod)}.";
            NPCID = npcID;
            Text = new Dictionary<int, IQuoteProvider>();
            ColorMethod = (parent, npcID, itemID) => Color.White;
        }

        public string GetQuote(NPC npc, int itemID)
        {
            if (Text.TryGetValue(itemID, out var result))
            {
                return result.GetQuote(this, npc.type, itemID);
            }

            if (FindDefaultText != null)
            {
                foreach (var defaultMethod in FindDefaultText)
                {
                    string defaultText = defaultMethod(itemID);
                    if (defaultText != null)
                        return defaultText;
                }
            }

            string key = DefaultKey + ShopQuotesMod.GetItemKeyName(itemID, Mod);
            string text = Language.GetTextValue(key);
            return key != text ? text : null;
        }

        public Color GetTextColor(int npcID, int itemID) {
            if (Text.TryGetValue(itemID, out var result))
                return result.GetTextColor(this, npcID, itemID);

            return ColorMethod(this, npcID, itemID);
        }

        public Color GetTextColor(NPC npc, int itemID)
        {
            return GetTextColor(npc.type, itemID);
        }

        public Asset<Texture2D> GetHeadTexture(NPC npc)
        {
            var t = HeadTexture?.Invoke();
            if (t != null)
                return t;

            int headType = TownNPCProfiles.Instance.GetProfile(npc, out var npcProfile)
                ? npcProfile.GetHeadTextureIndex(npc)
                : NPC.TypeToDefaultHeadIndex(npc.type);
            if (headType == -1)
                return null;
            return TextureAssets.NpcHead[headType];
        }

        public QuoteData SetQuote(int itemID, string customKey)
        {
            Text[itemID] = new QuoteProvider(customKey);
            return this;
        }

        public QuoteData SetQuote(int itemID, Func<string> customKey)
        {
            Text[itemID] = new FuncQuoteProvider(customKey);
            return this;
        }

        public QuoteData SetQuote(int itemID, string customKey, object[] parameters)
        {
            Text[itemID] = new FuncQuoteProvider(() => Language.GetTextValue(customKey, parameters));
            return this;
        }

        public QuoteData SetQuoteWith(int itemID, string customKey, object with)
        {
            Text[itemID] = new FuncQuoteProvider(() => Language.GetTextValueWith(customKey, with));
            return this;
        }

        public QuoteData SetQuoteBatch(int[] itemID, string customKey)
        {
            foreach (int i in itemID)
            {
                SetQuote(i, customKey);
            }
            return this;
        }

        public QuoteData SetQuoteBatch(int[] itemID, Func<string> customKey)
        {
            foreach (int i in itemID)
            {
                SetQuote(i, customKey);
            }
            return this;
        }

        public QuoteData SetQuoteBatch(int[] itemID, string customKey, object[] parameters)
        {
            foreach (int i in itemID)
            {
                SetQuote(i, customKey, parameters);
            }
            return this;
        }

        public QuoteData SetQuoteBatchWith(int[] itemID, string customKey, object with)
        {
            foreach (int i in itemID)
            {
                SetQuoteWith(i, customKey, with);
            }
            return this;
        }

        public QuoteData CustomHeadCheck(Func<Asset<Texture2D>> headTexture)
        {
            HeadTexture = headTexture;
            return this;
        }

        public QuoteData AddDefaultText(Func<int, string> findDefault)
        {
            (FindDefaultText ??= new List<Func<int, string>>()).Add(findDefault);
            return this;
        }

        public QuoteData UseColor(Func<QuoteData, int, int, Color> color)
        {
            ColorMethod = color;
            return this;
        }

        public QuoteData UseColor(Func<int, int, Color> color)
        {
            return UseColor((parent, npcID, itemID) => color(npcID, itemID));
        }

        public QuoteData UseColor(Func<Color> color)
        {
            return UseColor((parent, npcID, itemID) => color());
        }

        public QuoteData UseColor(Color color)
        {
            return UseColor((parent, npcID, itemID) => color);
        }
    }
}
