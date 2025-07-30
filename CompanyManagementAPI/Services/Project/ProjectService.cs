using CompanyManagementAPI.Data;
using CompanyManagementAPI.Domain;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Extensions;
using CompanyManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementAPI.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IRandomStringGeneratorService _codeService;

    public ProjectService(AppDbContext context, IRandomStringGeneratorService codeService)
    {
        _context = context;
        _codeService = codeService;
    }

    public async Task<IEnumerable<Project>> GetAllAsync() => await _context.Projects.ToListAsync();

    public async Task<Project?> GetByIdAsync(Guid id) => await _context.Projects.FindAsync(id);

    public async Task<Project> CreateAsync(ProjectDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Budget = dto.Budget,
                ProjectCode = string.Empty
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            
            string randomCode;
            try
            {
                randomCode = await _codeService.GenerateCodeAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(AppErrors.GenerateProjectFail.Description());
            }
            
            project.ProjectCode = $"{randomCode}-{project.Id}";
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return project;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException("Failed to create project in database", ex);
        }
    }

    public async Task<Project?> UpdateAsync(Guid id, ProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return null;

        project.Name = dto.Name;
        project.Budget = dto.Budget;

        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<decimal> GetTotalBudgetByDepartmentAsync(Guid departmentId)
    {
        var totalBudget = await _context.EmployeeProjects
            .Where(ep => ep.Employee.DepartmentId == departmentId)
            .Select(ep => ep.Project)
            .Distinct() 
            .SumAsync(p => p.Budget);

        return totalBudget;
    }
}