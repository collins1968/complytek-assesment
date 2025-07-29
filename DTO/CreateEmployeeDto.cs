namespace Company_ManagementAPI.DTO;

public class CreateEmployeeDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public Guid DepartmentId { get; set; }
}