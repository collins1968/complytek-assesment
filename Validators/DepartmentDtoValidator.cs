using Company_ManagementAPI.DTO;
using FluentValidation;

namespace Company_ManagementAPI.Validators;

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