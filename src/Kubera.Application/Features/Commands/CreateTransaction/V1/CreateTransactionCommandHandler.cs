using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Services;
using MediatR;
using Kubera.General.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Caching;
using System;

namespace Kubera.Application.Features.Commands.CreateTransaction.V1
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, IResult<TransactionModel>>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUserIdAccesor _userIdAccesor;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(IUserCacheService userCacheService, 
            ITransactionRepository transactionRepository,
            IAssetRepository assetRepository,
            IGroupRepository groupRepository,
            ICurrencyRepository currencyRepository,
            IUserIdAccesor userIdAccesor,
            IMapper mapper)
        {
            _userCacheService = userCacheService;
            _transactionRepository = transactionRepository;
            _assetRepository = assetRepository;
            _groupRepository = groupRepository;
            _currencyRepository = currencyRepository;
            _userIdAccesor = userIdAccesor;
            _mapper = mapper;
        }

        public async Task<IResult<TransactionModel>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                AssetId = request.Input.AssetId,
                Wallet = request.Input.Wallet,
                CreatedAt = DateTime.UtcNow,
                Date = request.Input.Date,
                Amount = request.Input.Amount,
                CurrencyId = request.Input.CurrencyId,
                Rate = request.Input.Rate,
                Fee = request.Input.Fee,
                FeeCurrencyId = request.Input.FeeCurrencyId,
                OwnerId = _userIdAccesor.Id
            };

            transaction = await _transactionRepository.Add(transaction, cancellationToken)
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

            var result = _mapper.Map<Transaction, TransactionModel>(transaction);

            if (currencies.Found(request.Input.CurrencyId, out var currency))
                result.Currency = _mapper.Map<Currency, CurrencyModel>(currency);

            if (assets.Found(request.Input.AssetId, out var asset))
            {
                result.Asset = _mapper.Map<Asset, AssetModel>(asset);


                if (groups.Found(asset.GroupId, out var group))
                    result.Asset.Group = _mapper.Map<Group, GroupModel>(group);
            }

            if (request.Input.FeeCurrencyId != null && currencies.Found(request.Input.FeeCurrencyId.Value, out var feeCurrency))
                result.FeeCurrency = _mapper.Map<Currency, CurrencyModel>(feeCurrency);

            _userCacheService.Remove(CacheRegion.Transactions);

            return result.AsResult();
        }
    }
}
