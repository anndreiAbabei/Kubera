using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Kubera.Data.Entities;
using Kubera.General.Services;
using Kubera.Data.Extensions;
using Kubera.General.Extensions;
using Kubera.General.Defaults;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Caching;
using System;

namespace Kubera.Application.Features.Queries.GetAssetGroupedTransactions.V1
{
    public class GetAssetGroupedTransactionsQueryHandler : CachingHandler<GetAssetGroupedTransactionsQuery, IEnumerable<GroupedTransactionsModel>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IForexService _forexService;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IDefaults _defaults;
        private readonly IMapper _mapper;

        public GetAssetGroupedTransactionsQueryHandler(IUserCacheService cacheService,
            IAssetRepository assetRepository,
            ITransactionRepository transactionRepository,
            IForexService forexService,
            IUserRepository userRepository,
            ICurrencyRepository currencyRepository,
            IDefaults defaults,
            IMapper mapper)
            : base(cacheService)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _forexService = forexService;
            _userRepository = userRepository;
            _currencyRepository = currencyRepository;
            _defaults = defaults;
            _mapper = mapper;

            cacheService.SetSlidingExpiration(TimeSpan.FromMinutes(10));
        }

        protected override async ValueTask<IResult<IEnumerable<GroupedTransactionsModel>>> HandleImpl(GetAssetGroupedTransactionsQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GroupedTransactionsModel>();
            IQueryable<Asset> query = _assetRepository.GetAll()
                .Include(a => a.Group);

            if (request.Filter != null)
            {
                if (request.Filter.AssetId.HasValue)
                    query = query.Where(t => t.Id == request.Filter.AssetId.Value);
                if (request.Filter.GroupId.HasValue)
                    query = query.Where(t => t.GroupId == request.Filter.GroupId.Value);
            }

            if (request.Order.HasValue)
                query = request.Order.Value == General.Models.Order.Ascending
                        ? query.OrderBy(a => a.Name)
                        : query.OrderByDescending(a => a.Name);

            var assets = await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var transactions = _transactionRepository.GetAll();
            var user = await _userRepository.GetMe(cancellationToken)
                .ConfigureAwait(false);
            var toCurrencyId = user.GetSettings()?.PrefferedCurrency;
            var toCurrency = toCurrencyId.HasValue
                    ? await _currencyRepository.GetById(toCurrencyId.Value, cancellationToken)
                    : null;
            var toCurrencyCode = toCurrency?.Code ?? _defaults.Currency;

            foreach (var asset in assets)
            {
                if (cancellationToken.IsCancellationRequested)
                    return await FromCancellationToken(cancellationToken);

                var assetTransactions = transactions.Where(t => t.AssetId == asset.Id);

                if (request.Filter != null)
                {
                    if (request.Filter.From.HasValue)
                        assetTransactions = assetTransactions.Where(t => t.CreatedAt >= request.Filter.From.Value);
                    if (request.Filter.To.HasValue)
                        assetTransactions = assetTransactions.Where(t => t.CreatedAt <= request.Filter.To.Value);
                }

                if (!await assetTransactions.AnyAsync(cancellationToken).ConfigureAwait(false))
                    continue;

                var priceValue = await _forexService.GetPriceOf(asset.Code, toCurrencyCode, cancellationToken)
                        .ConfigureAwait(false);

                var model = new GroupedTransactionsModel
                {
                    Asset = _mapper.Map<Asset, AssetModel>(asset),
                    Amount = await transactions.SumAsync(t => t.Amount, cancellationToken)
                        .ConfigureAwait(false),
                    TotalBought = await transactions.SumAsync(t => t.Amount * t.Rate, cancellationToken)
                        .ConfigureAwait(false),
                    ActualValue = priceValue.IsSuccess ? priceValue.Value.Rate : null
                };

                result.Add(model);
            }

            return result.AsResult();
        }

        protected override string GenerateKey(GetAssetGroupedTransactionsQuery request) => $"{base.GenerateKey(request)}.{request.Order}.{request.Filter}";
    }
}