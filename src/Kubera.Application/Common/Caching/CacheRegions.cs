using System;

namespace Kubera.Application.Common.Caching
{
    [Flags]
    internal enum CacheRegion : short
    {
        None = 1 << 0,
        Transactions = 1 << 1,
        Groups = 1 << 2,
        Assets = 1 << 3,
        Currencies = 1 << 4,
        Users = 1 << 5
    }
}
