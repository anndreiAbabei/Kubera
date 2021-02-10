namespace Kubera.General
{
    public static class Constraints
    {
        public static class SQL
        {
            public static class Default
            {
                public const string GetUtcDate = "getutcdate()";
            }
        }

        public static class Length 
        { 
            public static class Group
            {
                public const ushort Code = 32;
                public const ushort Name = 128;
            }

            public static class Currency
            {
                public const ushort Code = 4;
                public const ushort Name = 128;
                public const ushort Symbol = 5;
            }

            public static class Asset
            {
                public const ushort Code = 4;
                public const ushort Name = 128;
                public const ushort Symbol = 5;
            }
            public static class Transaction
            {
                public const ushort Wallet = 256;
            }
        }
    }
}
