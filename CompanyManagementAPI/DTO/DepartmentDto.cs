namespace CompanyManagementAPI.DTO;

public record DepartmentDto
{
    public required string Name { get; set; }
    public required string OfficeLocation { get; set; }
}