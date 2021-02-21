using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using Kubera.General.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetTransaction.V1
{
    public class GetTransactionQueryHandler : CachingHandler<GetTransactionQuery, TransactionModel>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public GetTransactionQueryHandler(IUserCacheService cacheService, 
            ITransactionRepository transactionRepository, 
            IMapper mapper, 
            IUserIdAccesor userIdAccesor)
            : base(cacheService)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        protected override async ValueTask<IResult<TransactionModel>> HandleImpl(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (transaction == null)
                return Result.Failure<TransactionModel>(ErrorCodes.NotFound);

            if (transaction.OwnerId != _userIdAccesor.Id)
                return Result.Failure<TransactionModel>(ErrorCodes.Forbid);

            return _mapper.Map<Transaction, TransactionModel>(transaction)
                .AsResult();
        }

        protected override string GenerateKey(GetTransactionQuery request) => $"{base.GenerateKey(request)}.{request.Id}";
    }
}
