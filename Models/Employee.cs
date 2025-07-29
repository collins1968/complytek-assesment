namespace Company_ManagementAPI.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal Salary { get; set; }
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } 
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

}