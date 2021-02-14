using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateAsset.V1
{
    class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
    {
        public UpdateAssetCommandValidator(IValidator<AssetUpdateModel> assetUpdateModelValidator)
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(assetUpdateModelValidator);
        }
    }
}