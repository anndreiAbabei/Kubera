using System.Collections.Generic;

namespace Kubera.Business.Entities.Defaults
{
    public static class Codes
    {
        public static class Group
        {
            public const string Commodity = "COMMODITY";
            public const string Crypto = "CRYPTO";
            public const string Stock = "STOCK";

            public static IEnumerable<(string Code, string Name)> Collection { get; } = new[]
            {
                (Commodity, nameof(Commodity)),
                (Crypto, nameof(Crypto)),
                (Stock, nameof(Stock)),
            };
        }

        public static class Currency
        {
            public const string USD = "USD";
            public const string EUR = "EUR";
            public const string GBP = "GBP";
            public const string RON = "RON";
            public const string SEK = "SEK";
            public const string NOK = "NOK";
            public const string DKK = "DKK";
            public const string CAD = "CAD";


            public static IEnumerable<(string Code, string Name, string Symbol)> Collection { get; } = new[]
            {
                (USD, "United States dollar", "US$"),
                (EUR, "Euro", "€"),
                (GBP, "Pound sterling", "£"),
                (RON, "Romanian leu", "L"),
                (SEK, "Swedish krona", "kr"),
                (NOK, "Norwegian krone", "kr"),
                (DKK, "Danish krone", "kr"),
                (CAD, "Canadian dollar", "C$"),
            };
        }

        public static class Asset
        {
            public const string XAU = "XAU";
            public const string XAG = "XAG";

            public const string BTC = "BTC";
            public const string XRP = "XRP";
            public const string ETH = "ETH";
            public const string XLM = "XLM";
            public const string LTC = "LTC";
            public const string ZRX = "ZRX";
            public const string BCH = "BCH";
            public const string EOS = "EOS";
            public const string XTZ = "XTZ";
            public const string OMG = "OMG";
            public const string DOGE = "DOGE";

            public const string ABNB = "ABNB";
            public const string GME = "GME";


            public static IEnumerable<(string Code, string Name, string Symbol, string GroupCode)> Collection { get; } = new[]
            {
                (XAU, "Gold", "G", Group.Commodity),
                (XAG, "Silver", "S", Group.Commodity),

                (BTC, "Bitcoin", "฿", Group.Crypto),
                (XRP, "Ripple", "R", Group.Crypto),
                (ETH, "Ethereum", "Ξ", Group.Crypto),
                (XLM, "Stellar", "S", Group.Crypto),
                (LTC, "Litecoin", "Ł", Group.Crypto),
                (ZRX, "0x", "0", Group.Crypto),
                (BCH, "Bitcoin Cash", "B", Group.Crypto),
                (EOS, "EOS", "E", Group.Crypto),
                (XTZ, "XTZ", "ꜩ", Group.Crypto),
                (OMG, "OMG", "O", Group.Crypto),
                (DOGE, "DOGE", "Ɖ", Group.Crypto),

                (ABNB, "AirBnb", "ABNB", Group.Stock),
                (GME, "GameStop", "GS", Group.Stock)
            };
        }
    }
}
