using FluentValidation;
using Kubera.Application.Common.Models;

namespace Kubera.Application.Features.Commands.CreateGroup
{
    public class GroupInputModelValidator : AbstractValidator<GroupInputModel>
    {
        public GroupInputModelValidator()
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
