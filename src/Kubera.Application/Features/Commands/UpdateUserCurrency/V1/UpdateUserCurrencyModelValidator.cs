using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateUserCurrency.V1
{
    public class UpdateUserCurrencyModelValidator : AbstractValidator<UpdateUserCurrencyModel>
    {
        public UpdateUserCurrencyModelValidator()
        {
            RuleFor(m => m.CurrencyId)
                .NotEmpty();
        }
    }
}
