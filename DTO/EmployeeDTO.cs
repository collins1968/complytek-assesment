using Company_ManagementAPI.Models;

namespace Company_ManagementAPI.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? DepartmentName { get; set; }

    public EmployeeDto(Employee e)
    {
        Id = e.Id;
        FullName = $"{e.FirstName} {e.LastName}";
        Email = e.Email;
        DepartmentName = e.Department?.Name;
    }
}