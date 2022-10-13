using Microsoft.Xna.Framework;

namespace ShopQuotesMod
{
    public interface IQuoteProvider
    {
        public int npc { get; set; }
        public int item { get; set; }

        /// <summary>
        /// Return null to prevent the shop quote from rendering
        /// </summary>
        /// <returns>Text which will be used to render an item's NPC Shop quote.</returns>
        string GetQuote();

        public Color GetTextColor();
    }
}