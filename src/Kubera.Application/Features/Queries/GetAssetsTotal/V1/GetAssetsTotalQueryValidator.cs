using FluentValidation;

namespace Kubera.Application.Features.Queries.GetAssetsTotal.V1
{
    public class GetAssetsTotalQueryValidator : AbstractValidator<GetAssetsTotalQuery>
    {
        public GetAssetsTotalQueryValidator()
        {
            RuleFor(q => q.CurrencyId)
                .NotEmpty();
        }
    }
}
