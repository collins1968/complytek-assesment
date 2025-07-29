using Company_ManagementAPI.DTO;
using FluentValidation;

namespace Company_ManagementAPI.Validators;

public class CreateEmployeeDtoValidator  : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}