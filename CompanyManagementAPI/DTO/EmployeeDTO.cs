using CompanyManagementAPI.Models;

namespace CompanyManagementAPI.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? DepartmentName { get; set; }

    public EmployeeDto(Employee e)
    {
        Id = e.Id;
        FullName = $"{e.FirstName} {e.LastName}";
        Email = e.Email;
        DepartmentName = e.Department?.Name;
    }
}