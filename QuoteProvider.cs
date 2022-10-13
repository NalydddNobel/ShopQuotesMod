using Terraria.Localization;

namespace ShopQuotesMod
{
    public class QuoteProvider : BaseQuoteProvider
    {
        public string Text;

        public QuoteProvider(QuoteData parent, int item, string text) : base(parent, item)
        {
            Text = text;
        }

        public override string GetQuote()
        {
            return Language.GetTextValue(Text);
        }
    }
}