using CompanyManagementAPI.Data;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using CompanyManagementAPI.Services;
using CompanyManagementAPITest.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementAPITest.Services;

public class DepartmentServiceTests
{
    private readonly AppDbContext _context;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _context = DbContextHelper.GetInMemoryDbContext();
        _service = new DepartmentService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDepartments()
    {
        _context.Departments.AddRange(
            new Department { Id = Guid.NewGuid(), Name = "HR", OfficeLocation = "Building A" },
            new Department { Id = Guid.NewGuid(), Name = "Engineering", OfficeLocation = "Building B" }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(d => d.Name == "HR");
        result.Should().Contain(d => d.Name == "Engineering");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectDepartment()
    {
        var departmentId = Guid.NewGuid();
        var department = new Department { Id = departmentId, Name = "Finance", OfficeLocation = "Building C" };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(departmentId);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Finance");
    }

    [Fact]
    public async Task CreateAsync_AddsDepartmentSuccessfully()
    {
        var dto = new DepartmentDto { Name = "Legal", OfficeLocation = "Building D" };

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be("Legal");
        result.OfficeLocation.Should().Be("Building D");

        var allDepartments = await _context.Departments.ToListAsync();
        allDepartments.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingDepartment()
    {
        var id = Guid.NewGuid();
        _context.Departments.Add(new Department { Id = id, Name = "Old Name", OfficeLocation = "Old Location" });
        await _context.SaveChangesAsync();

        var updateDto = new DepartmentDto { Name = "New Name", OfficeLocation = "New Location" };

        var result = await _service.UpdateAsync(id, updateDto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("New Name");
        result.OfficeLocation.Should().Be("New Location");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        var result = await _service.UpdateAsync(Guid.NewGuid(), new DepartmentDto { Name = "X", OfficeLocation = "Y" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DeletesDepartment()
    {
        var id = Guid.NewGuid();
        _context.Departments.Add(new Department { Id = id, Name = "To Delete", OfficeLocation = "Z" });
        await _context.SaveChangesAsync();

        var result = await _service.DeleteAsync(id);

        result.Should().BeTrue();
        var deleted = await _context.Departments.FindAsync(id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        var result = await _service.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }
}