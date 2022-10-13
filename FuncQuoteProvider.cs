using System;

namespace ShopQuotesMod
{
    public class FuncQuoteProvider : BaseQuoteProvider
    {
        public Func<string> Text;

        public FuncQuoteProvider(QuoteData parent, int item, Func<string> text) : base(parent, item)
        {
            Text = text;
        }

        public override string GetQuote()
        {
            return Text();
        }
    }
}