using CompanyManagementAPI.DTO;
using FluentValidation;

namespace CompanyManagementAPI.Validators;

public class DepartmentDtoValidator: AbstractValidator<DepartmentDto>
{
    public DepartmentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.");

        RuleFor(x => x.OfficeLocation)
            .NotEmpty().WithMessage("Office location is required.");
    }
}