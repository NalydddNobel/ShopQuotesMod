using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ShopQuotesMod {
    public partial class ShopQuotesMod : Mod {
        public static readonly Dictionary<Mod, string> AutomaticLocalizationPaths = new();

        public override object Call(params object[] args) {

            string callType = args[0] as string;

            if (string.IsNullOrEmpty(callType)) {
                return null;
            }

            return callType switch {
                "AddDefaultText" => AddDefaultText(args),
                "SetQuote" => SetQuote(args),
                "SetColor" => SetColor(args),
                "AddNPC" => AddNPC(args),
                "SetDefaultKey" => SetDefaultKey(args),
                _ => null,
            };
        }

        public static int GetInt(object obj) {
            if (obj is int _int) {
                return _int;
            }
            if (obj is short _short) {
                return _short;
            }
            return 0;
        }

        private object AddNPC(object[] args) {
            var mod = args[1] as Mod;
            int npcID = GetInt(args[2]);

            if (args.Length > 3 && args[3] is string defaultKey) {
                ModContent.GetInstance<QuoteDatabase>().AddOrGetNPC(npcID, mod, defaultKey);
                return true;
            }
            ModContent.GetInstance<QuoteDatabase>().AddOrGetNPC(npcID, mod);
            return true;
        }

        private object AddDefaultText(object[] args) {
            int npcID = GetInt(args[1]);

            if (!ModContent.GetInstance<QuoteDatabase>().TryGetValue(npcID, out var quote)) {
                return false;
            }

            if (args[2] is Func<int, string> key) {
                quote.AddDefaultText(key);
                return true;
            }

            return false;
        }

        private object SetQuote(object[] args) {
            int npcID = GetInt(args[1]);
            int itemType = GetInt(args[2]);

            if (!ModContent.GetInstance<QuoteDatabase>().TryGetValue(npcID, out var quote)) {
                return false;
            }

            if (args[3] is string key) {
                quote.SetQuote(itemType, key);
                return true;
            }
            if (args[3] is Func<string> keyMethod) {
                quote.SetQuote(itemType, keyMethod);
                return true;
            }

            return false;
        }

        private object SetColor(object[] args) {

            int npcID = GetInt(args[1]);

            if (!ModContent.GetInstance<QuoteDatabase>().TryGetValue(npcID, out var quote)) {
                return false;
            }

            if (args[2] is Color color) {
                quote.UseColor(color);
                return true;
            }
            if (args[2] is Func<Color> colorMethod) {
                quote.UseColor(colorMethod);
                return true;
            }
            if (args[2] is Func<int, int, Color> colorMethod2) {
                quote.UseColor(colorMethod2);
                return true;
            }
            if (args[2] is Func<QuoteData, int, int, Color> colorMethod3) {
                quote.UseColor(colorMethod3);
                return true;
            }
            return false;
        }

        private object SetDefaultKey(object[] args) {

            Mod mod = args[1] as Mod;
            string key = args[2] as string;

            if (string.IsNullOrEmpty(key)) {
                return false;
            }

            AutomaticLocalizationPaths[mod] = key;

            return true;
        }
    }
}