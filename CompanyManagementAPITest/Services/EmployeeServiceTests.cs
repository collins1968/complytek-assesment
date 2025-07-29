using CompanyManagementAPI.Data;
using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Models;
using CompanyManagementAPI.Services;
using CompanyManagementAPITest.TestHelpers;
using FluentAssertions;

namespace CompanyManagementAPITest.Services;

public class EmployeeServiceTests
{
    private readonly AppDbContext _context;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _context = DbContextHelper.GetInMemoryDbContext();
        _service = new EmployeeService(_context);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ShouldAddEmployee()
    {
        // Arrange
        var department = new Department { Id = Guid.NewGuid(), Name = "HR", OfficeLocation = "Nairobi" };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        var dto = new CreateEmployeeDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            DepartmentId = department.Id
        };

        // Act
        var result = await _service.CreateEmployeeAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Jane");
        result.DepartmentId.Should().Be(department.Id);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@example.com",
            Department = new Department { Name = "IT", OfficeLocation = "Remote" }
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetEmployeeByIdAsync(employee.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("john.smith@example.com");
        result.Department.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ShouldReturnAll()
    {
        // Arrange
        _context.Employees.AddRange(
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Walker",
                Email = "alice@company.com",
                Department = new Department { Name = "Finance", OfficeLocation = "Nairobi" }
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Brown",
                Email = "bob@company.com",
                Department = new Department { Name = "HR", OfficeLocation = "Nairobi" }
            }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllEmployeesAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldUpdateValues_WhenExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Old",
            LastName = "Name",
            Email = "old@email.com",
            Department = new Department { Name = "IT", OfficeLocation = "Remote" }
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var dto = new UpdateEmployeeDto
        {
            FirstName = "New",
            Email = "new@email.com"
        };

        // Act
        var result = await _service.UpdateEmployeeAsync(employee.Id, dto);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("New");
        result.Email.Should().Be("new@email.com");
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldRemoveEmployee_WhenExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Delete",
            LastName = "Me",
            Email = "deleteme@example.com",
            Department = new Department { Name = "IT", OfficeLocation = "Remote" }
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteEmployeeAsync(employee.Id);

        // Assert
        result.Should().BeTrue();
        var deleted = await _context.Employees.FindAsync(employee.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Act
        var result = await _service.DeleteEmployeeAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }
}