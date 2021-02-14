using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.App.Infrastructure.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.Data.Extensions;
using Kubera.General.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Kubera.General.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetTransactions.V1
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result<IEnumerable<TransactionModel>>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTransactionsQueryHandler(ITransactionRepository transactionRepository,
                                     IAssetRepository assetRepository,
                                     IGroupRepository groupRepository,
                                     ICurrencyRepository currencyRepository,
                                     IMapper mapper,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
            _assetRepository = assetRepository;
            _groupRepository = groupRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IEnumerable<TransactionModel>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
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

                _httpContextAccessor.HttpContext.AddPaging(new PagingResult(totalTrans));
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


            return models;
        }
    }
}
