using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.CreateGroup
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator(IValidator<GroupInputModel> createGroupInputModel)
        {
            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(createGroupInputModel);
        }
    }
}
