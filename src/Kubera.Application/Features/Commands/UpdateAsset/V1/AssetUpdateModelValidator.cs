using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateAsset.V1
{
    public class AssetUpdateModelValidator : AbstractValidator<AssetInputModel>
    {
        public AssetUpdateModelValidator()
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
