using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateTransaction.V1
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator(IValidator<TransactionUpdateModel> transactionUpdateModelValidator)
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(transactionUpdateModelValidator);
        }
    }
}
