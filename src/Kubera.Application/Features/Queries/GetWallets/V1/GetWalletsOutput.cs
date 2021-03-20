using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetWallets.V1
{
    public class GetWalletsOutput
    {
        public IEnumerable<string> Wallets { get; set; }
    }
}
