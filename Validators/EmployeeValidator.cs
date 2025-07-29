using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Models;
using FluentValidation;

namespace Company_ManagementAPI.Validators;

public class EmployeeValidator : AbstractValidator<UpdateEmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        // RuleFor(e => e.Salary).GreaterThan(0);
        RuleFor(e => e.DepartmentId).NotEmpty();
    }
    
}