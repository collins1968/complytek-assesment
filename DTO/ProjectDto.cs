namespace Company_ManagementAPI.DTO;

public class UpdateProjectDTO
{
    public string Name { get; set; } = default!;
    public int DepartmentId { get; set; }
}