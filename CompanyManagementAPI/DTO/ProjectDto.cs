namespace CompanyManagementAPI.DTO;

public record ProjectDto
{
    public required string Name { get; set; } 
    public decimal Budget { get; set; }
    // public string ProjectCode { get; set; } = string.Empty;
}