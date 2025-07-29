namespace Company_ManagementAPI.DTO;

public record ProjectDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    // public string ProjectCode { get; set; } = string.Empty;
}