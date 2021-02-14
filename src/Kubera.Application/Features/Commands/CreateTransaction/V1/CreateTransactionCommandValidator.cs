using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.CreateTransaction.V1
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator(IValidator<TransactionInputModel> createTransactionInputModel)
        {
            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(createTransactionInputModel);
        }
    }
}
