using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Kubera.General.Services;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Caching;
using System;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQueryHandler : CachingHandler<GetAssetsTotalQuery, GetAssetsTotalOutput>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IForexService _forexService;
        private readonly IMapper _mapper;

        public GetAssetsTotalQueryHandler(IUserCacheService cacheService, 
            IAssetRepository assetRepository, 
            ITransactionRepository transactionRepository,
            ICurrencyRepository currencyRepository,
            IForexService forexService,
            IMapper mapper)
            : base(cacheService)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _currencyRepository = currencyRepository;
            _forexService = forexService;
            _mapper = mapper;

            cacheService.SetSlidingExpiration(TimeSpan.FromMinutes(10));
        }

        protected override async ValueTask<IResult<GetAssetsTotalOutput>> HandleImpl(GetAssetsTotalQuery request, CancellationToken cancellationToken)
        {
            var assestsResult = new List<AssetTotalModel>();

            var assets = await GetAsseets(request, cancellationToken)
                .ConfigureAwait(false);
            var transactions = await GetTransactions(request, assets.Select(a => a.Id).ToArray(), cancellationToken)
                .ConfigureAwait(false);
            var currency = await _currencyRepository.GetById(request.CurrencyId, cancellationToken)
                .ConfigureAwait(false);

            foreach (var group in transactions.GroupBy(t => t.AssetId))
            {
                if (cancellationToken.IsCancellationRequested)
                    return await FromCancellationToken(cancellationToken);

                var asset = assets.FirstOrDefault(a => a.Id == group.Key);
                if (asset == null)
                    continue;

                decimal total = 0m;
                foreach (var transaction in group)
                {
                    var price = transaction.Amount * transaction.Rate;
                    total += transaction.CurrencyId != request.CurrencyId
                            ? await Exchange(transaction.Currency.Code, currency.Code, price, cancellationToken)
                                .ConfigureAwait(false)
                            : price;
                }
                var rate = await _forexService.GetPriceOf(asset.Code, currency.Code, cancellationToken)
                    .ConfigureAwait(false);
                var amount = group.Sum(t => t.Amount);
                decimal? totalNow = rate.IsSuccess ? amount * rate.Value.Rate : null;

                var model = new AssetTotalModel
                {
                    Id = asset.Id,
                    Code = asset.Code,
                    GroupId = asset.GroupId,
                    Icon = asset.Icon,
                    Name = asset.Name,
                    Order = asset.Order,
                    Symbol = asset.Symbol,
                    SumAmount = amount,
                    Total = total,
                    TotalNow = totalNow,
                    Increase = totalNow.HasValue ? total.ProcentFrom(totalNow.Value) : 0
                };

                if (asset.Group != null)
                    model.Group = _mapper.Map<Group, GroupModel>(asset.Group);

                assestsResult.Add(model);
            }

            var resultTotal = assestsResult.Select(a => a.Total).DefaultIfEmpty(0).Sum();
            var resultTotalNow = assestsResult.Select(a => a.TotalNow ?? 0).DefaultIfEmpty(0).Sum();

            var result = new GetAssetsTotalOutput
            {
                Assets = assestsResult,
                Count = assestsResult.Count,
                Total = resultTotal,
                TotalNow = resultTotalNow,
                Increase = resultTotal.ProcentFrom(resultTotalNow)
            };

            return result.AsResult();
        }

        protected override string GenerateKey(GetAssetsTotalQuery request) => $"{base.GenerateKey(request)}.{request.CurrencyId}.{request.Filter}";

        private async Task<IEnumerable<Asset>> GetAsseets(GetAssetsTotalQuery request, CancellationToken cancellationToken)
        {
            var query = _assetRepository.GetAll();

            if (request.Filter != null)
            {
                if (request.Filter.AssetId.HasValue)
                    query = query.Where(t => t.Id == request.Filter.AssetId.Value);
                if (request.Filter.GroupId.HasValue)
                    query = query.Where(t => t.GroupId == request.Filter.GroupId.Value);
            }

            return await query.Include(a => a.Group)
                            .ToListAsync(cancellationToken)
                            .ConfigureAwait(false);
        }

        private async Task<List<Transaction>> GetTransactions(GetAssetsTotalQuery request, Guid[] assetIds, CancellationToken cancellationToken)
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
                    query = query.Where(t => assetIds.Contains(t.AssetId));
            }

            return await query
                            .Include(t => t.Currency)
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
