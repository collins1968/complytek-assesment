namespace Company_ManagementAPI.Models;

public class EmployeeProject
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    
    public string Role { get; set; } = default!;
}