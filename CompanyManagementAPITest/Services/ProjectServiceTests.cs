using CompanyManagementAPI.Data;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using CompanyManagementAPI.Services;
using CompanyManagementAPITest.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CompanyManagementAPITest.Services;

public class ProjectServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IRandomStringGeneratorService> _mockCodeService;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _context = DbContextHelper.GetInMemoryDbContext();
        _mockCodeService = new Mock<IRandomStringGeneratorService>();
        _projectService = new ProjectService(_context, _mockCodeService.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    

    [Fact]
    public async Task GetAllAsync_WhenNoProjects_ReturnsEmptyList()
    {
        // Act
        var result = await _projectService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenProjectsExist_ReturnsAllProjects()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Name = "Project 1", Budget = 1000, ProjectCode = "CODE-1" },
            new Project { Id = Guid.NewGuid(), Name = "Project 2", Budget = 2000, ProjectCode = "CODE-2" },
            new Project { Id = Guid.NewGuid(), Name = "Project 3", Budget = 3000, ProjectCode = "CODE-3" }
        };

        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(projects.Select(p => p.Id).OrderBy(x => x), result.Select(p => p.Id).OrderBy(x => x));
    }
    

    [Fact]
    public async Task GetByIdAsync_WhenProjectExists_ReturnsProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project
        {
            Id = projectId,
            Name = "Test Project",
            Budget = 5000,
            ProjectCode = "TEST-CODE"
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.GetByIdAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal("Test Project", result.Name);
        Assert.Equal(5000, result.Budget);
        Assert.Equal("TEST-CODE", result.ProjectCode);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProjectDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _projectService.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task CreateAsync_WhenValidDto_CreatesAndReturnsProject()
    {
        // Arrange
        var dto = new ProjectDto
        {
            Name = "New Project",
            Budget = 10000
        };
        var generatedCode = "RAND123";

        _mockCodeService.Setup(x => x.GenerateCodeAsync())
            .ReturnsAsync(generatedCode);

        // Act
        var result = await _projectService.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Budget, result.Budget);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.StartsWith(generatedCode, result.ProjectCode);
        Assert.Contains(result.Id.ToString(), result.ProjectCode);

        // Verify project was saved to database
        var savedProject = await _context.Projects.FindAsync(result.Id);
        Assert.NotNull(savedProject);
        Assert.Equal(result.Name, savedProject.Name);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeGenerationFails_ShouldRollbackAndThrowWrappedException()
    {
        // Arrange
        var dto = new ProjectDto
        {
            Name = "New Project",
            Budget = 5000
        };

        _mockCodeService.Setup(x => x.GenerateCodeAsync())
            .ThrowsAsync(new InvalidOperationException("Code generation failed"));

        // Act
        var ex = await Assert.ThrowsAnyAsync<ApplicationException>(() => _projectService.CreateAsync(dto));

        // Assert
        Assert.Equal("Failed to create project in database", ex.Message);
        Assert.IsType<ApplicationException>(ex.InnerException);

        // Ensure project was not committed to DB
        var projectsInDb = await _context.Projects.ToListAsync();
        Assert.Empty(projectsInDb);
    }

    [Fact]
    public async Task CreateAsync_GeneratesUniqueProjectCode()
    {
        // Arrange
        var dto1 = new ProjectDto { Name = "Project 1", Budget = 1000 };
        var dto2 = new ProjectDto { Name = "Project 2", Budget = 2000 };
        var generatedCode = "CODE123";

        _mockCodeService.Setup(x => x.GenerateCodeAsync())
            .ReturnsAsync(generatedCode);

        // Act
        var project1 = await _projectService.CreateAsync(dto1);
        var project2 = await _projectService.CreateAsync(dto2);

        // Assert
        Assert.NotEqual(project1.ProjectCode, project2.ProjectCode);
        Assert.StartsWith(generatedCode, project1.ProjectCode);
        Assert.StartsWith(generatedCode, project2.ProjectCode);
    }

    [Fact]
    public async Task UpdateAsync_WhenProjectExists_UpdatesAndReturnsProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var originalProject = new Project
        {
            Id = projectId,
            Name = "Original Name",
            Budget = 5000,
            ProjectCode = "ORIG-CODE"
        };

        _context.Projects.Add(originalProject);
        await _context.SaveChangesAsync();

        var updateDto = new ProjectDto
        {
            Name = "Updated Name",
            Budget = 7000
        };

        // Act
        var result = await _projectService.UpdateAsync(projectId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal(7000, result.Budget);
        Assert.Equal("ORIG-CODE", result.ProjectCode); // ProjectCode should remain unchanged

        // Verify changes were saved to database
        var updatedProject = await _context.Projects.FindAsync(projectId);
        Assert.NotNull(updatedProject);
        Assert.Equal("Updated Name", updatedProject.Name);
        Assert.Equal(7000, updatedProject.Budget);
    }

    [Fact]
    public async Task UpdateAsync_WhenProjectDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateDto = new ProjectDto
        {
            Name = "Updated Name",
            Budget = 7000
        };

        // Act
        var result = await _projectService.UpdateAsync(nonExistentId, updateDto);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("", 1000)]
    [InlineData("Valid Name", 0)]
    [InlineData("Valid Name", -1000)]
    public async Task UpdateAsync_WithVariousInputs_UpdatesSuccessfully(string name, decimal budget)
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var originalProject = new Project
        {
            Id = projectId,
            Name = "Original Name",
            Budget = 5000,
            ProjectCode = "ORIG-CODE"
        };

        _context.Projects.Add(originalProject);
        await _context.SaveChangesAsync();

        var updateDto = new ProjectDto
        {
            Name = name,
            Budget = budget
        };

        // Act
        var result = await _projectService.UpdateAsync(projectId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(budget, result.Budget);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenProjectExists_DeletesProjectAndReturnsTrue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project
        {
            Id = projectId,
            Name = "Project to Delete",
            Budget = 5000,
            ProjectCode = "DEL-CODE"
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.DeleteAsync(projectId);

        // Assert
        Assert.True(result);

        // Verify project was deleted from database
        var deletedProject = await _context.Projects.FindAsync(projectId);
        Assert.Null(deletedProject);
    }

    [Fact]
    public async Task DeleteAsync_WhenProjectDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _projectService.DeleteAsync(nonExistentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenMultipleProjectsExist_DeletesOnlySpecifiedProject()
    {
        // Arrange
        var project1Id = Guid.NewGuid();
        var project2Id = Guid.NewGuid();
        
        var projects = new List<Project>
        {
            new Project { Id = project1Id, Name = "Project 1", Budget = 1000, ProjectCode = "CODE-1" },
            new Project { Id = project2Id, Name = "Project 2", Budget = 2000, ProjectCode = "CODE-2" }
        };

        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.DeleteAsync(project1Id);

        // Assert
        Assert.True(result);

        // Verify only project1 was deleted
        var remainingProjects = await _context.Projects.ToListAsync();
        Assert.Single(remainingProjects);
        Assert.Equal(project2Id, remainingProjects.First().Id);
    }
    

    [Fact]
    public async Task CreateUpdateDelete_Integration_WorksCorrectly()
    {
        // Arrange
        var createDto = new ProjectDto { Name = "Integration Test", Budget = 15000 };
        var updateDto = new ProjectDto { Name = "Updated Integration Test", Budget = 20000 };
        var generatedCode = "INT123";

        _mockCodeService.Setup(x => x.GenerateCodeAsync()).ReturnsAsync(generatedCode);

        // Act & Assert - Create
        var createdProject = await _projectService.CreateAsync(createDto);
        Assert.NotNull(createdProject);
        Assert.Equal(createDto.Name, createdProject.Name);
        Assert.Equal(createDto.Budget, createdProject.Budget);

        // Act & Assert - Update
        var updatedProject = await _projectService.UpdateAsync(createdProject.Id, updateDto);
        Assert.NotNull(updatedProject);
        Assert.Equal(updateDto.Name, updatedProject.Name);
        Assert.Equal(updateDto.Budget, updatedProject.Budget);
        Assert.Equal(createdProject.Id, updatedProject.Id);

        // Act & Assert - Delete
        var deleteResult = await _projectService.DeleteAsync(createdProject.Id);
        Assert.True(deleteResult);

        // Verify deletion
        var deletedProject = await _projectService.GetByIdAsync(createdProject.Id);
        Assert.Null(deletedProject);
    }


}