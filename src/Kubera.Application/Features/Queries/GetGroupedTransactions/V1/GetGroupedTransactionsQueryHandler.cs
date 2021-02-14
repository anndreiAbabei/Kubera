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

namespace Kubera.Application.Features.Queries.GetGroupedTransactions.V1 
{
    public class GetGroupedTransactionsQueryHandler : IRequestHandler<GetGroupedTransactionsQuery, Result<IEnumerable<GroupedTransactionsModel>>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetGroupedTransactionsQueryHandler(IAssetRepository assetRepository, ITransactionRepository transactionRepository,IMapper mapper)
        {
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<GroupedTransactionsModel>>> Handle(GetGroupedTransactionsQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var transactions = _transactionRepository.GetAll();
            var result = new List<GroupedTransactionsModel>();

            foreach(var asset in assets)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var assetTransactions = transactions.Where(t => t.AssetId == asset.Id);

                if (!await assetTransactions.AnyAsync(cancellationToken).ConfigureAwait(false))
                    continue;

                var model = new GroupedTransactionsModel
                {
                    Asset = _mapper.Map<Asset, AssetModel>(asset),
                    Amount = await transactions.SumAsync(t => t.Amount, cancellationToken)
                        .ConfigureAwait(false),
                    TotalBought = await transactions.SumAsync(t => t.Amount * t.Rate, cancellationToken)
                        .ConfigureAwait(false)
                };

                result.Add(model);
            }

            return result;
        }
    }
}