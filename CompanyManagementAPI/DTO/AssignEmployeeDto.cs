namespace CompanyManagementAPI.DTO;

public class AssignEmployeeDto
{
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public string Role { get; set; } = string.Empty;
}