using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;

namespace Kubera.Application.Features.Commands.UpdateUserCurrency.V1
{
    public class UpdateUserCurrencyCommand : IRequest<IResult>
    {
        public UpdateUserCurrencyModel Input { get; set; }
    }
}
