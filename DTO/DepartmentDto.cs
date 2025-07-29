namespace Company_ManagementAPI.DTO;

public record DepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
}