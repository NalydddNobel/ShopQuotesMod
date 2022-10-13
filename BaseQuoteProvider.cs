using Microsoft.Xna.Framework;

namespace ShopQuotesMod
{
    public abstract class BaseQuoteProvider : IQuoteProvider
    {
        public readonly QuoteData Parent;

        public int npc { get; set; }
        public int item { get; set; }

        public BaseQuoteProvider(QuoteData parent, int item)
        {
            Parent = parent;
            npc = parent.NPCid;
            this.item = item;
        }

        public abstract string GetQuote();

        public virtual Color GetTextColor()
        {
            return Parent.ColorMethod();
        }
    }
}
