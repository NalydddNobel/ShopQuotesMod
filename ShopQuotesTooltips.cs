using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ShopQuotesMod
{
    public class ShopQuotesTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.buy && item.tooltipContext == ItemSlot.Context.ShopItem && Main.LocalPlayer.talkNPC != -1)
            {
                try
                {
                    AddShopQuote(item, tooltips);
                }
                catch
                {
                }
            }
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name.StartsWith("Fake"))
            {
                return false;
            }
            if (line.Name == "ShopQuote")
            {
                DrawShopQuote(line, Main.npc[Main.LocalPlayer.talkNPC]);
                return false;
            }
            return true;
        }

        public void AddShopQuote(Item item, List<TooltipLine> tooltips)
        {
            var talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
            string text = null;
            var color = Color.White;
            var database = ModContent.GetInstance<QuoteDatabase>();

            if (database.TryGetValue(talkNPC.type, out var quotes))
            {
                try
                {
                    text = quotes.GetQuote(talkNPC, item.type);
                    color = quotes.GetTextColor(talkNPC, item.type);
                }
                catch (Exception e)
                {
                    Main.NewText(e.StackTrace);
                    Main.NewText(e.Message);
                }
            }
            if (text != null)
            {
                int index = 0;
                for (; index < tooltips.Count; index++)
                {
                    if (tooltips[index].Mod == "Terraria" && (tooltips[index].Name == "JourneyResearch" || tooltips[index].Name == "Price" || tooltips[index].Name == "SpecialPrice"))
                    {
                        break;
                    }
                }
                tooltips.Insert(index, new TooltipLine(Mod, "Fake", "⠀"));
                tooltips.Insert(index, new TooltipLine(Mod, "ShopQuote", FixNewlines(text)) { OverrideColor = color, });
            }
        }

        public static void DrawShopQuote(string text, int x, int y, float rotation, Vector2 origin, Vector2 baseScale, Color color, NPC npc)
        {
            var statusBubble = ModContent.Request<Texture2D>($"{nameof(ShopQuotesMod)}/StatusBubble").Value;
            var chatBubbleFrame = statusBubble.Frame();
            var chatBubbleScale = baseScale * 0.9f;
            chatBubbleScale.X *= 1.1f;
            var chatBubblePosition = new Vector2(x + chatBubbleFrame.Width / 2f * chatBubbleScale.X, y + chatBubbleFrame.Height / 2f * chatBubbleScale.Y);

            var headTexture = GetHeadTexture(npc);

            if (headTexture != null)
            {
                Main.spriteBatch.Draw(statusBubble, chatBubblePosition + new Vector2(2f) * chatBubbleScale,
                    chatBubbleFrame, Color.Black * 0.4f, 0f, chatBubbleFrame.Size() / 2f, chatBubbleScale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(statusBubble, chatBubblePosition,
                    chatBubbleFrame, Color.White, 0f, chatBubbleFrame.Size() / 2f, chatBubbleScale, SpriteEffects.None, 0f);
                if (headTexture.IsLoaded)
                {
                    var headFrame = headTexture.Value.Frame();
                    var headOrigin = headFrame.Size() / 2f;

                    Main.spriteBatch.Draw(headTexture.Value, chatBubblePosition + new Vector2(2f),
                        headFrame, Color.Black * 0.4f, 0f, headOrigin, 1f, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(headTexture.Value, chatBubblePosition,
                        headFrame, Color.White, 0f, headOrigin, 1f, SpriteEffects.None, 0f);
                }
            }


            if (ModLoader.TryGetMod("TrueTooltips", out _))
            {
                return;
            }

            var font = FontAssets.MouseText.Value;
            string measurementString = text;
            if (text.Contains('\n'))
            {
                measurementString = text.Split('\n')[0];
            }
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, new Vector2(x, chatBubblePosition.Y - font.MeasureString(measurementString).Y / 2f), color, rotation, origin, baseScale);
        }
        public static void DrawShopQuote(string text, int x, int y, Color color, NPC npc)
        {
            DrawShopQuote(text, x, y, 0f, Vector2.Zero, Vector2.One, color, npc);
        }
        public static void DrawShopQuote(DrawableTooltipLine line, NPC npc)
        {
            DrawShopQuote(line.Text, line.X, line.Y, line.Rotation, line.Origin, line.BaseScale, line.OverrideColor.GetValueOrDefault(line.Color), npc);
        }

        private static Asset<Texture2D> GetHeadTexture(NPC npc)
        {
            return ModContent.GetInstance<QuoteDatabase>()[npc.type].GetHeadTexture(npc);
        }

        public static string FixNewlines(string text)
        {
            var lines = text.Split('\n');
            int maxCharacters = Main.screenWidth / 10;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > maxCharacters)
                {
                    int period = 0;
                    var punctuation = lines[i].Split(". ");

                    for (int j = 0; j < punctuation.Length - 1; j++)
                    {
                        punctuation[j] += ". ";
                    }

                    punctuation = FurtherSplit(", ", punctuation);
                    punctuation = FurtherSplit("- ", punctuation);
                    punctuation = FurtherSplit("! ", punctuation);
                    lines[i] = string.Empty;
                    for (int j = 0; j < punctuation.Length; j++)
                    {
                        if (period + punctuation[j].Length >= maxCharacters)
                        {
                            if (j != 0)
                            {
                                lines[i] += '\n' + "         ";
                            }
                            lines[i] += punctuation[j];
                            period = 0;
                        }
                        else
                        {
                            lines[i] += punctuation[j];
                            period += lines[i].Length;
                        }
                    }
                }
                lines[i] = "         " + lines[i];
            }

            text = string.Join('\n', lines);
            if (text.Trim().EndsWith('\n'))
            {
                text.Remove(text.Length - 2, 1);
            }

            return text;
        }

        public static string[] FurtherSplit(string value, string[] text)
        {
            var t = new List<string>();
            foreach (var s in text)
            {
                var t2 = s.Split(value);
                for (int i = 0; i < t2.Length - 1; i++)
                {
                    t2[i] += value;
                }
                t.AddRange(t2);
            }
            return t.ToArray();
        }
    }
}