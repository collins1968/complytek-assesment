using CompanyManagementAPI.DTO;
using FluentValidation;

namespace CompanyManagementAPI.Validators;

public class ProjectDtoValidator: AbstractValidator<ProjectDto>
{
    public ProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.");

        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be greater than zero.");
    }
}