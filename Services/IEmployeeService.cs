using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Models;

namespace Company_ManagementAPI.Services;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(Guid id);
}