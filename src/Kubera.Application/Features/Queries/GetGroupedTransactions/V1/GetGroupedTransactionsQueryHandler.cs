using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Kubera.Application.Features.Queries.GetGroupedTransactions.V1 
{
    public class GetGroupedTransactionsQueryHandler : IRequestHandler<GetGroupedTransactionsQuery, Result<IEnumerable<GroupedTransactions>>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupedTransactionsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public Task<Result<IEnumerable<GroupedTransactions>>> Handle(GetGroupedTransactionsQuery request, CancellationToken cancellationToken)
        {
            _groupRepository.GetAll()
        }
    }
}