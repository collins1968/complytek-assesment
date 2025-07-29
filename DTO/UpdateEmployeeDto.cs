namespace Company_ManagementAPI.DTO;

public class UpdateEmployeeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Guid? DepartmentId { get; set; }
    
}