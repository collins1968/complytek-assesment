using CompanyManagementAPI.Models;

namespace CompanyManagementAPI.DTO;

public record ProjectDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public decimal Budget { get; set; }
    public string ProjectCode { get; set; }
}