using AutoMapper;
using CSharpFunctionalExtensions;
using Kubera.Application.Common;
using Kubera.Application.Common.Extensions;
using Kubera.Application.Common.Models;
using Kubera.Application.Services;
using Kubera.Data.Entities;
using Kubera.General.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Features.Queries.GetCurrencies.V1
{
    public class GetCurrenciesQueryHandler : CachingHandler<GetCurrenciesQuery, IEnumerable<CurrencyModel>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetCurrenciesQueryHandler(IUserCacheService cacheService, ICurrencyRepository currencyRepository, IMapper mapper)
            : base(cacheService)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        protected override async ValueTask<IResult<IEnumerable<CurrencyModel>>> HandleImpl(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAll()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return currencies.Select(_mapper.Map<Currency, CurrencyModel>)
                .AsResult();
        }
    }
}
