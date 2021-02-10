using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetCurrencies.V1
{
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, Result<IEnumerable<CurrencyModel>>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<CurrencyModel>>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return currencies.Select(_mapper.Map<Currency, CurrencyModel>)
                .AsResult();
        }
    }
}
