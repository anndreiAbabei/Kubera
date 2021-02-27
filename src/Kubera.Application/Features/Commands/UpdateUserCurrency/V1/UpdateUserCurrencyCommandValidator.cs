using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateUserCurrency.V1
{
    public class UpdateUserCurrencyCommandValidator : AbstractValidator<UpdateUserCurrencyCommand>
    {
        public UpdateUserCurrencyCommandValidator(IValidator<UpdateUserCurrencyModel> updateUserCurrencyModel)
        {
            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(updateUserCurrencyModel);
        }
    }
}
