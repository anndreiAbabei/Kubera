using FluentValidation;

namespace Kubera.Application.Features.Queries.GetTransaction.V1
{
    public class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
    {
        public GetTransactionQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty();
        }
    }
}
