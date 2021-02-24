using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    public class GetGroupTotalQueryHandler : CachingHandler<GetGroupTotalQuery, IEnumerable<GroupTotalModel>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IForexService _forexService;

        public GetGroupTotalQueryHandler(IUserCacheService cacheService, 
            IGroupRepository groupRepository,
            ITransactionRepository transactionRepository,
            ICurrencyRepository currencyRepository,
            IForexService forexService)
            : base(cacheService)
        {
            _groupRepository = groupRepository;
            _transactionRepository = transactionRepository;
            _currencyRepository = currencyRepository;
            _forexService = forexService;

            cacheService.SetSlidingExpiration(TimeSpan.FromMinutes(10));
        }

        protected override async ValueTask<IResult<IEnumerable<GroupTotalModel>>> HandleImpl(GetGroupTotalQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GroupTotalModel>();
            var groups = await _groupRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var currency = await _currencyRepository.GetById(request.CurrencyId, cancellationToken)
                .ConfigureAwait(false);
            var transactions = await _transactionRepository
                .GetAll()
                .Include(t => t.Currency)
                .Include(t => t.Asset)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var transGroup in transactions.GroupBy(t => t.Asset.GroupId))
            {
                if (cancellationToken.IsCancellationRequested)
                    return await Task.FromCanceled<IResult<IEnumerable<GroupTotalModel>>>(cancellationToken);

                var group = groups.FirstOrDefault(a => a.Id == transGroup.Key);
                if (group == null)
                    continue;

                decimal total = 0m;
                decimal totalNow = 0m;
                foreach (var transaction in transGroup)
                {
                    var price = transaction.Amount * transaction.Rate;
                    total += transaction.CurrencyId != request.CurrencyId
                            ? await Exchange(transaction.Currency.Code, currency.Code, price, cancellationToken)
                                .ConfigureAwait(false)
                            : price;
                }
                foreach (var tranAsseet in transGroup.GroupBy(t => t.AssetId))
                {
                    var asset = tranAsseet.First().Asset;
                    var rate = await _forexService.GetPriceOf(asset.Code, currency.Code, cancellationToken)
                        .ConfigureAwait(false);
                    totalNow += rate.IsSuccess ? tranAsseet.Sum(t => t.Amount) * rate.Value.Rate : 0;
                }    
                var amount = transGroup.Sum(t => t.Amount);

                var model = new GroupTotalModel
                {
                    Id = group.Id,
                    Code = group.Code,
                    Name = group.Name,
                    SumAmount = amount,
                    Total = total,
                    TotalNow = totalNow != 0 ? totalNow : null,
                    Increase = CalculateProcent(total, totalNow)
                };

                result.Add(model);
            }

            return result.AsResult();
        }

        protected override string GenerateKey(GetGroupTotalQuery request) => $"{base.GenerateKey(request)}.{request.CurrencyId}";

        private async ValueTask<decimal> Exchange(string from, string to, decimal amount, CancellationToken cancellationToken = default)
        {
            if (amount == 0)
                return amount;

            var rate = await _forexService.GetPriceOf(from, to, cancellationToken)
                    .ConfigureAwait(false);

            return rate.IsSuccess ? amount * rate.Value.Rate : amount;
        }

        private static float CalculateProcent(decimal previous, decimal current)
        {
            if (previous == 0)
                return 0f;

            if (current == 0)
                return -100f;

            return (float)((current - previous) / previous * 100m);
        }
    }
}
