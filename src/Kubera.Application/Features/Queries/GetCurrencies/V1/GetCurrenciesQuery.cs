using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Kubera.Application.Features.Queries.GetCurrencies.V1
{
    public class GetCurrenciesQuery : IRequest<Result<IEnumerable<CurrencyModel>>>
    {
    }
}
