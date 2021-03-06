using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
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
    public class GetGroupTotalQueryHandler : CachingHandler<GetGroupTotalQuery, GetGroupTotalOutput>
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

        protected override async ValueTask<IResult<GetGroupTotalOutput>> HandleImpl(GetGroupTotalQuery request, CancellationToken cancellationToken)
        {
            var groupResults = new List<GroupTotalModel>();

            var currency = await _currencyRepository.GetById(request.CurrencyId, cancellationToken)
                .ConfigureAwait(false);
            var groups = await GetGroups(request, cancellationToken)
                .ConfigureAwait(false);
            var transactions = await GetTransactions(request, groups.Select(g => g.Id).ToArray(), cancellationToken)
                .ConfigureAwait(false);

            foreach (var transGroup in transactions.GroupBy(t => t.Asset.GroupId))
            {
                if (cancellationToken.IsCancellationRequested)
                    return await FromCancellationToken(cancellationToken);

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
                    Increase = total.ProcentFrom(totalNow)
                };

                groupResults.Add(model);
            }

            var resultTotal = groupResults.Select(a => a.Total).DefaultIfEmpty(0).Sum();
            var resultTotalNow = groupResults.Select(a => a.TotalNow ?? 0).DefaultIfEmpty(0).Sum();


            var result = new GetGroupTotalOutput
            {
                Groups = groupResults,
                Count = groupResults.Count,
                Total = resultTotal,
                TotalNow = resultTotalNow,
                Increase = resultTotal.ProcentFrom(resultTotalNow)
            };

            return result.AsResult();
        }

        protected override string GenerateKey(GetGroupTotalQuery request) => $"{base.GenerateKey(request)}.{request.CurrencyId}.{request.Filter}";

        private async Task<IEnumerable<Group>> GetGroups(GetGroupTotalQuery request, CancellationToken cancellationToken)
        {
            var query = _groupRepository.GetAll();

            if (request.Filter != null)
            {
                if (request.Filter.AssetId.HasValue)
                    query = query.Where(t => t.Assets.Select(a => a.Id).Contains(request.Filter.AssetId.Value));
                if (request.Filter.GroupId.HasValue)
                    query = query.Where(t => t.Id == request.Filter.GroupId.Value);
            }

            return await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<List<Transaction>> GetTransactions(GetGroupTotalQuery request, Guid[] groupIds, CancellationToken cancellationToken = default)
        {
            var query = _transactionRepository.GetAll();

            if (request.Filter != null)
            {
                if (request.Filter.From.HasValue)
                    query = query.Where(t => t.CreatedAt >= request.Filter.From.Value);
                if (request.Filter.To.HasValue)
                    query = query.Where(t => t.CreatedAt <= request.Filter.To.Value);
                if (request.Filter.AssetId.HasValue)
                    query = query.Where(t => t.AssetId == request.Filter.AssetId.Value);
                if (request.Filter.GroupId.HasValue)
                    query = query.Where(t => t.Asset.GroupId == request.Filter.GroupId.Value);
                else
                    query = query.Where(t => groupIds.Contains(t.Asset.GroupId));
            }

            return await query.Include(t => t.Currency)
                            .Include(t => t.Asset)
                            .ToListAsync(cancellationToken)
                            .ConfigureAwait(false);
        }

        private async ValueTask<decimal> Exchange(string from, string to, decimal amount, CancellationToken cancellationToken = default)
        {
            if (amount == 0)
                return amount;

            var rate = await _forexService.GetPriceOf(from, to, cancellationToken)
                    .ConfigureAwait(false);

            return rate.IsSuccess ? amount * rate.Value.Rate : amount;
        }
    }
}
