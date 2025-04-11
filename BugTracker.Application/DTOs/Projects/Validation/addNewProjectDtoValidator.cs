using BugTracker.Application.DTOs.Projects.DTO;
using FluentValidation;

namespace BugTracker.Application.DTOs.Projects.Validation
{
    public class addNewProjectDtoValidator : AbstractValidator<ProjectDtoRequest>
    {
        public addNewProjectDtoValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Project name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Project description is required.");

            RuleFor(x => x.StartDate)
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Start date must be in the future.");

            RuleFor(x => x.EndDate)
                .Must((project, endDate) => endDate > project.StartDate)
                .WithMessage("End date must be after start date.");
        }
    }
}
