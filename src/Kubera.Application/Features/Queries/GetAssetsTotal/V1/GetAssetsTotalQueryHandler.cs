using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Kubera.General.Services;
using Kubera.Application.Common.Extensions;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQueryHandler : IRequestHandler<GetAssetsTotalQuery, IResult<IEnumerable<AssetTotalModel>>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IForexService _forexService;
        private readonly IMapper _mapper;

        public GetAssetsTotalQueryHandler(IAssetRepository assetRepository, 
            ITransactionRepository transactionRepository,
            ICurrencyRepository currencyRepository,
            IForexService forexService,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _currencyRepository = currencyRepository;
            _forexService = forexService;
            _mapper = mapper;
        }

        public async Task<IResult<IEnumerable<AssetTotalModel>>> Handle(GetAssetsTotalQuery request, CancellationToken cancellationToken)
        {
            var result = new List<AssetTotalModel>();
            var assets = await _assetRepository.GetAll()
                .Include(a => a.Group)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var currency = await _currencyRepository.GetById(request.CurrencyId, cancellationToken)
                .ConfigureAwait(false);
            var transactions = await _transactionRepository
                .GetAll()
                .Include(t => t.Currency)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach(var group in transactions.GroupBy(t => t.AssetId))
            {
                if (cancellationToken.IsCancellationRequested)
                    return await Task.FromCanceled<IResult<IEnumerable<AssetTotalModel>>>(cancellationToken);

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
                    Increase = totalNow.HasValue ? CalculateProcent(total, totalNow.Value) : 0
                };

                if (asset.Group != null)
                    model.Group = _mapper.Map<Group, GroupModel>(asset.Group);

                result.Add(model);
            }

            return result.AsResult();

        }

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
