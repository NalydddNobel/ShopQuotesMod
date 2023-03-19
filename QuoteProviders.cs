using Microsoft.Xna.Framework;
using System;
using Terraria.Localization;

namespace ShopQuotesMod {
    public interface IQuoteProvider {
        /// <summary>
        /// Return null to prevent the shop quote from rendering
        /// </summary>
        /// <returns>Text which will be used to render an item's NPC Shop quote.</returns>
        string GetQuote(QuoteData quoteParent, int npcID, int itemType);

        public Color GetTextColor(QuoteData quoteParent, int npcID, int itemType);
    }

    public abstract class BaseQuoteProvider : IQuoteProvider {
        public abstract string GetQuote(QuoteData parent, int npcID, int itemType);

        public virtual Color GetTextColor(QuoteData parent, int npcID, int itemType) {
            return parent.ColorMethod(parent, npcID, itemType);
        }
    }

    public class QuoteProvider : BaseQuoteProvider {
        public string Text;

        public QuoteProvider(string text) : base() {
            Text = text;
        }

        public override string GetQuote(QuoteData parent, int npcID, int itemType) {
            return Language.GetTextValue(Text);
        }
    }

    public class FuncQuoteProvider : BaseQuoteProvider {
        public Func<string> Text;

        public FuncQuoteProvider(Func<string> text) : base() {
            Text = text;
        }

        public override string GetQuote(QuoteData parent, int npcID, int itemType) {
            return Text();
        }
    }
}