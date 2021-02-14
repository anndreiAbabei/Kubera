using FluentValidation;

namespace Kubera.Application.Features.Commands.DeleteAsset.V1
{
    public class DeleteAssetCommandValidator : AbstractValidator<DeleteAssetCommand>
    {
        public DeleteAssetCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}
