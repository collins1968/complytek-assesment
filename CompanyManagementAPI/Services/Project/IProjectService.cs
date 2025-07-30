using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;

namespace CompanyManagementAPI.Services;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Project> CreateAsync(ProjectDto dto);
    Task<Project?> UpdateAsync(Guid id, ProjectDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<decimal> GetTotalBudgetByDepartmentAsync(Guid departmentId);
}