using CompanyManagementAPI.Data;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementAPI.Services;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;
    public DepartmentService(AppDbContext context) => _context = context;

    public async Task<List<Department>> GetAllAsync() =>
        await _context.Departments.ToListAsync();

    public async Task<Department?> GetByIdAsync(Guid id) =>
        await _context.Departments.FindAsync(id);

    public async Task<Department> CreateAsync(DepartmentDto dto)
    {
        var department = new Department { Id = new Guid(),Name = dto.Name, OfficeLocation = dto.OfficeLocation };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<Department?> UpdateAsync(Guid id, DepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null) return null;

        department.Name = dto.Name;
        department.OfficeLocation = dto.OfficeLocation;

        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null) return false;

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }
}