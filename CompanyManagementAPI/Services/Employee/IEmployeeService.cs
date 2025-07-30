using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;

namespace CompanyManagementAPI.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(Guid id);
    Task<bool> RemoveEmployeeFromProjectAsync(Guid employeeId, Guid projectId);
    Task<List<ProjectDto>> GetProjectsForEmployeeAsync(Guid employeeId);
    Task<bool> AssignEmployeeToProjectAsync(AssignEmployeeDto dto);
}