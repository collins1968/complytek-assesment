using System.ComponentModel.DataAnnotations;

namespace CompanyManagementAPI.DTO;

public class CreateEmployeeDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public required string Email { get; set; } 
    public Guid DepartmentId { get; set; }
}