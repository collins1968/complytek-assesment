using CompanyManagementAPI.DTO;
using FluentValidation;

namespace CompanyManagementAPI.Validators;

public class AssignEmployeeDtoValidator : AbstractValidator<AssignEmployeeDto>
{
    public AssignEmployeeDtoValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("EmployeeId is required.");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId is required.");
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required.")
            .When(x => x.Role != null); 
    }
}