using Company_ManagementAPI.Data;
using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Company_ManagementAPI.Services;

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
        string projectCode;
        try
        {
            projectCode = await _codeService.GenerateCodeAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Failed to generate random code");
        }
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var projectId = Guid.NewGuid();
            
            var project = new Project
            {
                Id = projectId,
                Name = dto.Name,
                Budget = dto.Budget,
                ProjectCode = $"{projectCode}-{projectId}"
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return project;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException("Failed to create project in database", e);
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
}