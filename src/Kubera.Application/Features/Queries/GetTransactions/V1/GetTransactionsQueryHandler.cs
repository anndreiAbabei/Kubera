using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Kubera.General.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Kubera.Application.Common.Extensions;
using Kubera.General.Services;
using Kubera.Application.Common;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsQueryHandler : CachingHandler<GetTransactionsQuery, GetTransactionsQueryOutput>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(IUserCacheService cacheService,
            ITransactionRepository transactionRepository,
            IAssetRepository assetRepository,
            IGroupRepository groupRepository,
            ICurrencyRepository currencyRepository,
            IMapper mapper)
            : base(cacheService)
        {
            _transactionRepository = transactionRepository;
            _assetRepository = assetRepository;
            _groupRepository = groupRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        protected override async ValueTask<IResult<GetTransactionsQueryOutput>> HandleImpl(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            IPagingResult paging = null;
            var order = request.Order ?? Order.Descending;

            var query = _transactionRepository.GetAll();

            if(request.Date != null)
            {
                if (request.Date.From.HasValue)
                    query = query.Where(t => t.CreatedAt >= request.Date.From.Value);
                if (request.Date.To.HasValue)
                    query = query.Where(t => t.CreatedAt <= request.Date.To.Value);
            }

            if (request.Paging != null)
            {
                var totalTrans = await query.CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                query = query.Skip((int)(request.Paging.Page * request.Paging.Items))
                    .Take((int)request.Paging.Items);

                paging = new PagingResult(totalTrans);
            }

            query = order == Order.Descending
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt);

            var transactions = await query.ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var currencies = await _currencyRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var assets = await _assetRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var groups = await _groupRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);


            var models = transactions.Select(_mapper.Map<Transaction, TransactionModel>).ToList();

            foreach (var model in models)
            {
                if (currencies.Found(model.CurrencyId, out var currency))
                    model.Currency = _mapper.Map<Currency, CurrencyModel>(currency);

                if (assets.Found(model.AssetId, out var asset))
                {
                    model.Asset = _mapper.Map<Asset, AssetModel>(asset);


                    if (groups.Found(asset.GroupId, out var group))
                        model.Asset.Group = _mapper.Map<Group, GroupModel>(group);
                }

                if (model.FeeCurrencyId.HasValue && currencies.Found(model.FeeCurrencyId.Value, out var feeCurrency))
                    model.FeeCurrency = _mapper.Map<Currency, CurrencyModel>(feeCurrency);
            }
            
            var output = new GetTransactionsQueryOutput
            {
                Transactions = models,
                Paging = paging
            };

            return output.AsResult();
        }

        protected override string GenerateKey(GetTransactionsQuery request)
        {
            return base.GenerateKey(request) + "." +
                request.Date + "." +
                request.Paging + "." +
                request.Order;
        }
    }
}
