using FluentValidation;

namespace Kubera.Application.Features.Commands.DeleteGroup.V1
{
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}
