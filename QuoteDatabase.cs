using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ShopQuotesMod
{
    /// <summary>
    /// Holds information about NPC shop quotes.
    /// </summary>
    public class QuoteDatabase : ModSystem
    {
        private readonly Dictionary<int, QuoteData> database = new();

        public override void Load()
        {
            database.Clear();

            AddNPC(NPCID.TravellingMerchant)
                .AddDefaultText((i) =>
                {
                    if (ContentSamples.ItemsByType.TryGetValue(i, out var item) && 
                        (Main.vanityPet[item.buffType] || Main.lightPet[item.buffType]))
                    {
                        return Language.GetTextValue("Mods.ShopQuotesMod.TravellingMerchant.AnyPet");
                    }
                    return null;
                })
                .AddDefaultText((i) =>
                {
                    if (ContentSamples.ItemsByType.TryGetValue(i, out var item) && item.frontSlot > 0)
                    {
                        return Language.GetTextValue("Mods.ShopQuotesMod.TravellingMerchant.AnyCloak");
                    }
                    return null;
                })
                .SetQuoteBatch(new int[] { ItemID.StarPrincessCrown, ItemID.StarPrincessDress, ItemID.CelestialWand }, "Mods.ShopQuotesMod.TravellingMerchant.CelestialWandAndStarPrincessSet")
                .SetQuoteBatch(new int[] { ItemID.GameMasterShirt, ItemID.GameMasterPants, }, () =>
                {
                    return Language.GetTextValue($"Mods.ShopQuotesMod.TravellingMerchant.GameMasterSet{(Main.masterMode ? "_Master" : "")}");
                })
                .SetQuoteBatch(new int[] { ItemID.ChefHat, ItemID.ChefShirt, ItemID.ChefPants }, () => Language.GetTextValueWith("Mods.ShopQuotesMod.TravellingMerchant.ChefSet", new { World = Main.worldName, }))
                .UseColor(new Color(130, 128, 255));

            AddNPC(NPCID.Merchant)
                .SetQuote(ItemID.PiggyBank, () => Language.GetTextValue($"Mods.ShopQuotesMod.Merchant.PiggyBank{(Main.GetMoonPhase() == MoonPhase.Full && LanternNight.LanternsUp ? "_Goober" : "")}"))
                .SetQuote(ItemID.DiscoBall, () =>
                {
                    string partyGirl = NPC.GetFirstNPCNameOrNull(NPCID.PartyGirl);
                    if (partyGirl != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.Merchant.DiscoBall_PartyGirl", new { PartyGirl = partyGirl, });
                    }
                    return Language.GetTextValue($"Mods.ShopQuotesMod.Merchant.DiscoBall");
                })
                .SetQuote(ItemID.Torch, "LegacyDialog.5")
                .UseColor(Color.Yellow);
            AddNPC(NPCID.ArmsDealer)
                .SetQuote(ItemID.Minishark, "LegacyDialog.66")
                .SetQuote(ItemID.MusketBall, "LegacyDialog.67")
                .SetQuote(ItemID.Nail, () =>
                {
                    string merchant = NPC.GetFirstNPCNameOrNull(NPCID.Merchant);
                    if (merchant != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.ArmsDealer.Nail_Merchant", new { Merchant = merchant, });
                    }
                    return Language.GetTextValue($"Mods.ShopQuotesMod.ArmsDealer.Nail");
                })
                .SetQuoteBatch(new int[] { ItemID.NurseHat, ItemID.NurseShirt, ItemID.NursePants, }, () =>
                {
                    string nurse = NPC.GetFirstNPCNameOrNull(NPCID.Nurse);
                    if (nurse != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.ArmsDealer.NurseOutfit_Nurse", new { Nurse = nurse, });
                    }
                    return Language.GetTextValue($"Mods.ShopQuotesMod.ArmsDealer.NurseOutfit");
                })
                .UseColor(Color.Gray * 1.45f);
            AddNPC(NPCID.Demolitionist)
                .SetQuote(ItemID.Bomb, () => Language.GetTextValueWith("LegacyDialog.93", new { WorldEvilStone = WorldGen.crimson ? Language.GetTextValue("Misc.Crimstone") : Language.GetTextValue("Misc.Ebonstone"), }))
                .SetQuote(ItemID.Dynamite, "LegacyDialog.101")
                .UseColor(Color.Gray * 1.45f);
            AddNPC(NPCID.GoblinTinkerer)
                .UseColor(new Color(200, 70, 105, 255));
            AddNPC(NPCID.Wizard)
                .UseColor(Color.BlueViolet * 1.5f);
            AddNPC(NPCID.Mechanic)
                .SetQuoteBatch(new int[] { ItemID.BlueWrench, ItemID.GreenWrench, ItemID.YellowWrench, }, "Mods.ShopQuotesMod.Mechanic.ColoredWrench")
                .SetQuoteBatch(new int[] { ItemID.RedPressurePlate, ItemID.GreenPressurePlate, ItemID.GrayPressurePlate, ItemID.BrownPressurePlate, ItemID.BluePressurePlate, ItemID.YellowPressurePlate, ItemID.OrangePressurePlate, }, "Mods.ShopQuotesMod.Mechanic.PressurePlates")
                .SetQuote(ItemID.MechanicsRod, () =>
                {
                    string angler = NPC.GetFirstNPCNameOrNull(NPCID.Angler);
                    if (angler != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.Mechanic.MechanicsRod", new { Angler = angler, });
                    }
                    return null;
                })
                .UseColor(Color.Lerp(Color.Red, Color.White, 0.33f));
            AddNPC(NPCID.Truffle)
                .UseColor(Color.Lerp(Color.Blue, Color.White, 0.5f));
            AddNPC(NPCID.DD2Bartender)
                .UseColor(Color.Lerp(Color.Orange, Color.White, 0.66f));
            AddNPC(NPCID.SkeletonMerchant)
                .CustomHeadCheck(() => ModContent.Request<Texture2D>($"{nameof(ShopQuotesMod)}/SkeletonMerchantHead", AssetRequestMode.ImmediateLoad))
                .SetQuoteBatch(new int[] { ItemID.BlueCounterweight, ItemID.RedCounterweight, ItemID.PurpleCounterweight, ItemID.GreenCounterweight, }, "Mods.ShopQuotesMod.SkeletonMerchant.Counterweights")
                .UseColor(Color.Gray * 1.2f);
            AddNPC(NPCID.Golfer)
                .SetQuoteBatch(new int[] { ItemID.GolfCupFlagBlue, ItemID.GolfCupFlagGreen, ItemID.GolfCupFlagPurple, ItemID.GolfCupFlagRed, ItemID.GolfCupFlagWhite, ItemID.GolfCupFlagYellow, }, "Mods.ShopQuotesMod.Golfer.GolfCupFlag")
                .SetQuoteBatch(new int[] { ItemID.GolfHat, ItemID.GolfVisor, ItemID.GolfShirt, ItemID.GolfPants, }, "Mods.ShopQuotesMod.Golfer.GolfOutfit")
                .SetQuoteBatch(new int[] { ItemID.GolfPainting1, ItemID.GolfPainting2, ItemID.GolfPainting3, ItemID.GolfPainting4, }, "Mods.ShopQuotesMod.Golfer.GolfPaintings")
                .UseColor(Color.Lerp(Color.SkyBlue, Color.White, 0.5f));
            AddNPC(NPCID.DyeTrader)
                .SetQuote(ItemID.SilverDye, () =>
                {
                    var arr = Main.LocalPlayer.armor;
                    string helmetName = null;
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        if (arr[i] != null && !arr[i].IsAir && arr[i].headSlot == Main.LocalPlayer.head)
                        {
                            helmetName = arr[i].Name;
                            break;
                        }
                    }
                    if (helmetName != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.DyeTrader.SilverDye_Helmet", new { Helmet = helmetName, });
                    }
                    return Language.GetTextValue($"Mods.ShopQuotesMod.DyeTrader.SilverDye");
                })
                .UseColor(Color.Lerp(Color.BlueViolet, Color.White, 0.5f));
            AddNPC(NPCID.PartyGirl)
                .SetQuoteBatch(new int[] { ItemID.RedRocket, ItemID.GreenRocket, ItemID.BlueRocket, ItemID.YellowRocket, }, "Mods.ShopQuotesMod.PartyGirl.ColoredRockets")
                .SetQuoteBatch(new int[] { ItemID.SillyStreamerPink, ItemID.SillyStreamerGreen, ItemID.SillyStreamerBlue, }, "Mods.ShopQuotesMod.PartyGirl.ColoredStreamers")
                .SetQuoteBatch(new int[] { ItemID.SillyBalloonPurple, ItemID.SillyBalloonGreen, ItemID.SillyBalloonPink, }, "Mods.ShopQuotesMod.PartyGirl.SillyBalloons")
                .SetQuoteBatch(new int[] { ItemID.SillyBalloonTiedGreen, ItemID.SillyBalloonTiedPurple, ItemID.SillyBalloonTiedPink, }, "Mods.ShopQuotesMod.PartyGirl.SillyTiedBalloons")
                .UseColor(Color.HotPink);
            AddNPC(NPCID.Cyborg)
                .UseColor(Color.Cyan * 1.5f);
            AddNPC(NPCID.Painter)
                .UseColor(() => ShopQuotesMod.MaxRGBA(ShopQuotesMod.LerpBetween(new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Lime, Color.Green, Color.Teal, Color.Cyan, Color.SkyBlue, Color.Blue, Color.Purple, Color.Violet, Color.Pink, },
                Main.GlobalTimeWrappedHourly * 0.08f), 100));
            AddNPC(NPCID.WitchDoctor)
                .UseColor(Color.GreenYellow);
            AddNPC(NPCID.Pirate)
                .UseColor(Color.Orange * 1.2f);
            AddNPC(NPCID.BestiaryGirl)
                .CustomHeadCheck(() => Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].altTexture == 2 ? ModContent.Request<Texture2D>($"{nameof(ShopQuotesMod)}/ZoologistAltHead", AssetRequestMode.ImmediateLoad) : null)
                .UseColor(() => Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].altTexture == 2 ? Color.Red : new Color(255, 140, 160, 255));
            AddNPC(NPCID.Princess)
                .SetQuote(ItemID.GlassSlipper, () => Language.GetTextValue($"Mods.ShopQuotesMod.Princess.GlassSlipper{(Main.LocalPlayer.Male ? "_Male" : "_Female")}"))
                .SetQuote(ItemID.MusicBoxCredits, () => Language.GetTextValue($"Mods.ShopQuotesMod.Princess.MusicBoxCredits{(NPC.GetFirstNPCNameOrNull(NPCID.Guide) == "Andrew" ? "_GuideAndrew" : "")}"))
                .SetQuote(ItemID.FlaskofParty, () =>
                {
                    string partyGirl = NPC.GetFirstNPCNameOrNull(NPCID.PartyGirl);
                    if (partyGirl != null)
                    {
                        return Language.GetTextValueWith($"Mods.ShopQuotesMod.Princess.FlaskofParty_PartyGirl", new { PartyGirl = partyGirl, });
                    }
                    return Language.GetTextValue($"Mods.ShopQuotesMod.Princess.FlaskofParty");
                })
                .UseColor(Main.creativeModeColor * 1.25f);
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("Fargowiltas", out var fargowiltas))
            {
                try
                {
                    AddNPC(fargowiltas.Find<ModNPC>("Squirrel").Type)
                        .AddDefaultText((i) => Language.GetTextValue("Mods.Mods.ShopQuotesMod.Fargowiltas_Squirrel"))
                        .UseColor(Color.Gray * 1.66f);
                }
                catch
                {
                }
            }
        }

        public QuoteData this[int npc]
        {
            get => database[npc];
        }

        public bool TryGetValue(int npc, out QuoteData quote)
        {
            return database.TryGetValue(npc, out quote);
        }

        /// <summary>
        /// Adds an NPC entry. If one doesn't exist already, it will be added to the database.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        public QuoteData AddOrGetNPC(int npc, Mod mod, string defaultKey = null)
        {
            if (TryGetValue(npc, out var quote))
            {
                return quote;
            }
            if (string.IsNullOrEmpty(defaultKey)
                && ShopQuotesMod.AutomaticLocalizationPaths.TryGetValue(mod, out string defaultKeyOverride)) {
                defaultKey = defaultKeyOverride;
            }
            database.Add(npc, new QuoteData(mod, npc, defaultKey ?? $"Mods.{mod.Name}."));
            return this[npc];
        }
        internal QuoteData AddNPC(int npc, string defaultKey = null)
        {
            return AddOrGetNPC(npc, ModContent.GetInstance<ShopQuotesMod>(), defaultKey);
        }
    }
}