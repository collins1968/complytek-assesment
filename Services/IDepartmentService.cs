using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Models;

namespace Company_ManagementAPI.Services;

public interface IDepartmentService
{
    Task<List<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(Guid id);
    Task<Department> CreateAsync(DepartmentDto dto);
    Task<Department?> UpdateAsync(Guid id, DepartmentDto dto);
    Task<bool> DeleteAsync(Guid id);
}