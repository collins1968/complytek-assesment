using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Company_ManagementAPI.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees").WithTags("Employees");

        group.MapGet("/", async (IEmployeeService service) =>
        {
            var employees = await service.GetAllEmployeesAsync();
            if (!employees.Any()) return Results.NoContent();

            var dtos = employees.Select(e => new EmployeeDto(e)).ToList();
            return Results.Ok(dtos);
        });

        group.MapGet("/{id:Guid}", async (Guid id, IEmployeeService service) =>
        {
            var employee = await service.GetEmployeeByIdAsync(id);
            return employee is null ? Results.NotFound() : Results.Ok(new EmployeeDto(employee));
        });

        group.MapPost("/",
            async (CreateEmployeeDto dto, IValidator<CreateEmployeeDto> validator, IEmployeeService service) =>
            {
                var validation = await validator.ValidateAsync(dto);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(errors);
                }

                var employee = await service.CreateEmployeeAsync(dto);
                return Results.Created($"/api/employees/{employee.Id}", new EmployeeDto(employee));
            });

        group.MapPut("/{id:Guid}",
            async (Guid id, UpdateEmployeeDto dto, IValidator<UpdateEmployeeDto> validator, IEmployeeService service) =>
            {
                var validation = await validator.ValidateAsync(dto);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(errors);
                }

                var updated = await service.UpdateEmployeeAsync(id, dto);
                return updated is null ? Results.NotFound() : Results.Ok(new EmployeeDto(updated));
            });

        group.MapDelete("/{id:Guid}", async (Guid id, IEmployeeService service) =>
        {
            var deleted = await service.DeleteEmployeeAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}