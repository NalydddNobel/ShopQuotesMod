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
        public int NPCid { get; set; }
        public readonly string DefaultKey;
        public Func<Color> ColorMethod;
        public Func<Asset<Texture2D>> HeadTexture;

        public List<Func<string>> FindDefaultText;

        private readonly Dictionary<int, IQuoteProvider> Text;
        private readonly Mod Mod;

        internal QuoteData(Mod mod, int npcID, string defaultKey = "Mods.ShopQuotesMod.")
        {
            DefaultKey = $"{defaultKey}{ShopQuotesMod.GetNPCKeyName(npcID)}.";
            NPCid = npcID;
            Mod = mod;
            Text = new Dictionary<int, IQuoteProvider>();
            ColorMethod = () => Color.White;
        }

        public string GetQuote(NPC npc, int itemID)
        {
            if (Text.TryGetValue(itemID, out var result))
            {
                return result.GetQuote();
            }

            if (FindDefaultText != null)
            {
                foreach (var defaultMethod in FindDefaultText)
                {
                    string defaultText = defaultMethod();
                    if (defaultText != null)
                        return defaultText;
                }
            }

            string key = DefaultKey + ShopQuotesMod.GetItemKeyName(itemID, Mod);
            string text = Language.GetTextValue(key);
            return key != text ? text : null;
        }

        public Color GetTextColor(NPC npc, int itemID)
        {
            if (Text.TryGetValue(itemID, out var result))
                return result.GetTextColor();

            return ColorMethod();
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
            Text[itemID] = new QuoteProvider(this, itemID, customKey);
            return this;
        }

        public QuoteData SetQuote(int itemID, Func<string> customKey)
        {
            Text[itemID] = new FuncQuoteProvider(this, itemID, customKey);
            return this;
        }

        public QuoteData SetQuote(int itemID, string customKey, object[] parameters)
        {
            Text[itemID] = new FuncQuoteProvider(this, itemID, () => Language.GetTextValue(customKey, parameters));
            return this;
        }

        public QuoteData SetQuoteWith(int itemID, string customKey, object with)
        {
            Text[itemID] = new FuncQuoteProvider(this, itemID, () => Language.GetTextValueWith(customKey, with));
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

        public QuoteData AddDefaultText(Func<string> findDefault)
        {
            if (FindDefaultText == null)
                FindDefaultText = new List<Func<string>>();

            FindDefaultText.Add(findDefault);
            return this;
        }

        public QuoteData UseColor(Func<Color> color)
        {
            ColorMethod = color;
            return this;
        }

        public QuoteData UseColor(Color color)
        {
            ColorMethod = () => color;
            return this;
        }
    }
}
