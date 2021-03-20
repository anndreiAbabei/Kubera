using FluentValidation;
using Kubera.Application.Common.Models;
using System;

namespace Kubera.Application.Features.Commands.CreateTransaction.V1
{
    public class TransactionInputModelValidator : AbstractValidator<TransactionInputModel>
    {
        public TransactionInputModelValidator()
        {
            RuleFor(m => m.AssetId)
                .NotEmpty();

            RuleFor(m => m.Wallet)
                .NotNull()
                .NotEmpty();

            RuleFor(m => m.Date)
                .NotEmpty()
                .LessThan(DateTime.Now.AddDays(1));

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
