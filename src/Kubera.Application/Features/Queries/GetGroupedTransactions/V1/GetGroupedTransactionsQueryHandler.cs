using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;
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

namespace Kubera.Application.Features.Queries.GetGroupedTransactions.V1 
{
    public class GetGroupedTransactionsQueryHandler : IRequestHandler<GetGroupedTransactionsQuery, Result<IEnumerable<GroupedTransactionsModel>>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IForexService _forexService;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IDefaults _defaults;
        private readonly IMapper _mapper;

        public GetGroupedTransactionsQueryHandler(IAssetRepository assetRepository,
            ITransactionRepository transactionRepository,
            IForexService forexService,
            IUserRepository userRepository,
            ICurrencyRepository currencyRepository,
            IDefaults defaults,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _forexService = forexService;
            _userRepository = userRepository;
            _currencyRepository = currencyRepository;
            _defaults = defaults;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<GroupedTransactionsModel>>> Handle(GetGroupedTransactionsQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GroupedTransactionsModel>();
            var assets = await _assetRepository.GetAll()
                .Include(a => a.Group)
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
                cancellationToken.ThrowIfCancellationRequested();
                var assetTransactions = transactions.Where(t => t.AssetId == asset.Id);

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
                    ActualValue = priceValue.Rate
                };

                result.Add(model);
            }

            return result;
        }
    }
}