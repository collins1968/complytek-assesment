using CompanyManagementAPI.Data;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementAPI.Services;

public class EmployeeService: IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DepartmentId = dto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<Employee?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.FirstName)) employee.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.LastName)) employee.LastName = dto.LastName;
        if (!string.IsNullOrWhiteSpace(dto.Email)) employee.Email = dto.Email;
        if (dto.DepartmentId.HasValue) employee.DepartmentId = dto.DepartmentId.Value;

        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AssignEmployeeToProjectAsync(AssignEmployeeDto dto)
    {
        var exists = await _context.EmployeeProjects
            .AnyAsync(ep => ep.EmployeeId == dto.EmployeeId && ep.ProjectId == dto.ProjectId);

        if (exists) return false;

        var assignment = new EmployeeProject
        {
            EmployeeId = dto.EmployeeId,
            ProjectId = dto.ProjectId,
            Role = dto.Role
        };

        _context.EmployeeProjects.Add(assignment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveEmployeeFromProjectAsync(Guid employeeId, Guid projectId)
    {
        var assignment = await _context.EmployeeProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

        if (assignment == null) return false;

        _context.EmployeeProjects.Remove(assignment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ProjectDto>> GetProjectsForEmployeeAsync(Guid employeeId)
    {
        return await _context.EmployeeProjects
            .Where(ep => ep.EmployeeId == employeeId)
            .Include(ep => ep.Project)
            .Select(ep => new ProjectDto
            {
                Id = ep.Project.Id,
                Name = ep.Project.Name,
                ProjectCode = ep.Project.ProjectCode,
                Budget = ep.Project.Budget
            })
            .ToListAsync();
    }
}