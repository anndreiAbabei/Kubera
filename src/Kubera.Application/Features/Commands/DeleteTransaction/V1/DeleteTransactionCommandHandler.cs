using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Features.Queries.GetTransactions.V1;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.DeleteTransaction.V1
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, IResult>
    {
        private readonly IUserCacheService _userCacheService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public DeleteTransactionCommandHandler(IUserCacheService userCacheService,
            ITransactionRepository transactionRepository,
            IUserIdAccesor userIdAccesor)
        {
            _userCacheService = userCacheService;
            _transactionRepository = transactionRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<IResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (transaction == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (transaction.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            await _transactionRepository.Delete(transaction.Id, cancellationToken)
                .ConfigureAwait(false);

            _userCacheService.RemoveAll<TransactionModel>();
            _userCacheService.RemoveAll<GroupedTransactionsModel>();
            _userCacheService.RemoveAll<GetTransactionsQueryOutput>();
            _userCacheService.RemoveAll<IEnumerable<TransactionModel>>();
            _userCacheService.RemoveAll<IEnumerable<GroupedTransactionsModel>>();

            return Result.Success();
        }
    }
}
