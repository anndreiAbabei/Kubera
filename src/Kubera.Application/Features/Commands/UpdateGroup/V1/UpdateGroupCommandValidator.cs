using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateGroup.V1
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupCommandValidator(IValidator<GroupUpdateModel> groupUpdateModelValidator)
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Input)
                .NotNull()
                .SetValidator(groupUpdateModelValidator);
        }
    }
}
