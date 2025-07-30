namespace CompanyManagementAPI.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Budget { get; set; }
    public string ProjectCode { get; set; } = default!;
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
}