using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.CreateAsset.V1
{
    public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
    {
        public CreateAssetCommandValidator(IValidator<AssetInputModel> assetInputModelValidator)
        {
            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(assetInputModelValidator);
        }
    }
}
