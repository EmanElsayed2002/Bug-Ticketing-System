using BugTracker.Application.DTOs.Bug.DTO;
using FluentValidation;

namespace BugTracker.Application.DTOs.Bug.Validation
{
    public class BugREquestDtoValidation : AbstractValidator<AddBugDTO>
    {
        public BugREquestDtoValidation()
        {

            RuleFor(x => x.Title)
              .NotEmpty().WithMessage("Bug name is required.");

            RuleFor(x => x.Desc)
                .NotEmpty().WithMessage("Project description is required.");

            RuleFor(x => x.CreatedDate)
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Start date must be in the future.");



        }
    }
}
