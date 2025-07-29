using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using FluentValidation;

namespace CompanyManagementAPI.Validators;

public class EmployeeValidator : AbstractValidator<UpdateEmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.DepartmentId).NotEmpty();
    }
    
}