using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Services;
using Kubera.General.Extensions;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Commands.DeleteTransaction.V1
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserIdAccesor _userIdAccesor;

        public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository,
                                     IUserIdAccesor userIdAccesor)
        {
            _transactionRepository = transactionRepository;
            _userIdAccesor = userIdAccesor;
        }

        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (transaction == null)
                return Result.Failure(ErrorCodes.NotFound);

            if (transaction.OwnerId != _userIdAccesor.Id)
                return Result.Failure(ErrorCodes.Forbid);

            await _transactionRepository.Delete(transaction.Id, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
