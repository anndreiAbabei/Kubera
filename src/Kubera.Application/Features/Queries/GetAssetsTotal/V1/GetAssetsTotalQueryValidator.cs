using FluentValidation;

namespace Kubera.Application.Features.Queries.etAssetsTotal.V1
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
