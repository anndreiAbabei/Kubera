using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateTransaction.V1
{
    public class TransactionUpdateModelValidator : AbstractValidator<TransactionUpdateModel>
    {
        public TransactionUpdateModelValidator()
        {
            RuleFor(m => m.AssetId)
                .NotEmpty();

            RuleFor(m => m.Wallet)
                .NotNull()
                .NotEmpty();

            RuleFor(m => m.CreatedAt)
                .NotEmpty();

            RuleFor(m => m.Amount)
                .NotEqual(0m);

            RuleFor(m => m.CurrencyId)
                .NotEmpty();

            RuleFor(m => m.Rate)
                .NotEqual(0m)
                .GreaterThan(0m);
        }
    }
}
