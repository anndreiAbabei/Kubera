using CSharpFunctionalExtensions;
using Kubera.Application.Common.Caching;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.UpdateTransaction.V1
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public UpdateTransactionCommandHandler(IUserCacheService userCacheService,
            ITransactionRepository transactionRepository,
            IUserIdAccesor userIdAccesor)
        {
            _userCacheService = userCacheService;
            _transactionRepository = transactionRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (transaction == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (transaction.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            transaction.AssetId = request.Input.AssetId;
            transaction.Wallet = request.Input.Wallet;
            transaction.Amount = request.Input.Amount;
            transaction.CurrencyId = request.Input.CurrencyId;
            transaction.Rate = request.Input.Rate;
            transaction.Fee = request.Input.Fee;
            transaction.FeeCurrencyId = request.Input.FeeCurrencyId;

            await _transactionRepository.Update(transaction, cancellationToken)
                    .ConfigureAwait(false);

            _userCacheService.Remove(CacheRegion.Transactions);

            return Result.Success();
        }
    }
}
