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

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQueryHandler : IRequestHandler<GetAssetsTotalQuery, Result<IEnumerable<AssetTotalModel>>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetAssetsTotalQueryHandler(IAssetRepository assetRepository, 
            ITransactionRepository transactionRepository,
            ICurrencyRepository currencyRepository,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AssetTotalModel>>> Handle(GetAssetsTotalQuery request, CancellationToken cancellationToken)
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
                .GroupBy(t => t.AssetId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach(var group in transactions) 
            {
                if(cancellationToken.IsCancellationRequested)
                    return await Task.FromCanceled<Result<IEnumerable<AssetTotalModel>>>(cancellationToken);

                var asset = assets.FirstOrDefault(a => a.Id == group.Key);
                if(asset == null)
                    continue;

                decimal total = 0m;
                decimal totalNow = 0m; //todo
                foreach(var transaction in group)
                {
                    var price = transaction.Amount * transaction.Rate;
                    total += transaction.CurrencyId != request.CurrencyId
                            ? await Exchange(transaction.Currency.Code, currency.Code, price)
                                .ConfigureAwait(false)
                            : price;
                }

                var model = new AssetTotalModel
                {
                    Id = asset.Id,
                    Code = asset.Code,
                    GroupId = asset.GroupId,
                    Icon = asset.Icon,
                    Name = asset.Name,
                    Order = asset.Order,
                    Symbol = asset.Symbol,
                    Total = total,
                    TotalNow = totalNow, 
                    Increase = 100/total*totalNow
                };

                if(asset.Group != null)
                    model.Group = _mapper.Map<Group, GroupModel>(asset.Group);

                result.Add(model);
            }

            return result;

        }

        private async ValueTask<decimal> Exchange(string from, string to, decimal amount)
        {
            return amount;
        }
    }
}
