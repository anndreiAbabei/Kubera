using FluentValidation;

namespace Kubera.Application.Features.Queries.GetGroup.V1
{
    public class GetGroupQueryValidator : AbstractValidator<GetGroupQuery>
    {
        public GetGroupQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}
