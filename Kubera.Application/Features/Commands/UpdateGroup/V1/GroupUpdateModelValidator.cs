using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.UpdateGroup.V1
{
    public class GroupUpdateModelValidator : AbstractValidator<GroupUpdateModel>
    {
        public GroupUpdateModelValidator()
        {
            RuleFor(i => i.Code)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(16);

            RuleFor(i => i.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(128);
        }
    }
}
