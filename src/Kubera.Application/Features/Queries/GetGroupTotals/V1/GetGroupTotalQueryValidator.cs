using FluentValidation;

namespace Kubera.Application.Features.Queries.GetGroupTotals.V1
{
    public class GetGroupTotalQueryValidator : AbstractValidator<GetGroupTotalQuery>
    {
        public GetGroupTotalQueryValidator()
        {
            RuleFor(q => q.CurrencyId)
                .NotEmpty();
        }
    }
}
