using FluentValidation;

namespace Kubera.Application.Features.Queries.GetAsset.V1
{
    public class GetAssetQueryValidator : AbstractValidator<GetAssetQuery>
    {
        public GetAssetQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}
