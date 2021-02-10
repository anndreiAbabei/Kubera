using FluentValidation;

namespace Kubera.Application.Features.Commands.DeleteTransaction.V1
{
    public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}
