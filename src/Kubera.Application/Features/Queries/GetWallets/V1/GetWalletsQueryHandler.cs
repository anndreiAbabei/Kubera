using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Services;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetWallets.V1
{
    public class GetWalletsQueryHandler : CachingHandler<GetWalletsQuery, GetWalletsOutput>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetWalletsQueryHandler(IUserCacheService cacheService,
            ITransactionRepository transactionRepository)
            : base(cacheService)
        {
            _transactionRepository = transactionRepository;

            cacheService.SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));
        }

        protected override async ValueTask<IResult<GetWalletsOutput>> HandleImpl(GetWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _transactionRepository.GetAll()
                .OrderByDescending(t => t.CreatedAt)
                .Select(a => a.Wallet)
                .Distinct()
                .ToListAsync(cancellationToken);

            return new GetWalletsOutput
            {
                Wallets = wallets
            }.AsResult();
        }
    }
}
