namespace CompanyManagementAPI.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OfficeLocation { get; set; }

    public List<Employee> Employees { get; set; }
}