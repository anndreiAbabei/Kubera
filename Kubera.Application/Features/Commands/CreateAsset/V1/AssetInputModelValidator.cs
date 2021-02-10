using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.CreateAsset.V1
{
    public class AssetInputModelValidator : AbstractValidator<AssetInputModel>
    {
        public AssetInputModelValidator()
        {
            RuleFor(i => i.Code)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(16);

            RuleFor(i => i.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(128);

            RuleFor(i => i.Symbol)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(16);

            RuleFor(i => i.GroupId)
                .NotEmpty();
        }
    }
}
