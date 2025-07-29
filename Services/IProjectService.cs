using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Models;

namespace Company_ManagementAPI.Services;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Project> CreateAsync(ProjectDto dto);
    Task<Project?> UpdateAsync(Guid id, ProjectDto dto);
    Task<bool> DeleteAsync(Guid id);
}