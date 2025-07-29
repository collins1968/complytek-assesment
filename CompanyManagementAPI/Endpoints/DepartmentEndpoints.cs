using CompanyManagementAPI.DTO;
using CompanyManagementAPI.Services;
using FluentValidation;

namespace CompanyManagementAPI.Endpoints;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/departments").WithTags("Departments");

        group.MapGet("/", async (IDepartmentService service) =>
            Results.Ok(await service.GetAllAsync()));

        group.MapGet("/{id:Guid}", async (Guid id, IDepartmentService service) =>
        {
            var dept = await service.GetByIdAsync(id);
            return dept is null ? Results.NotFound() : Results.Ok(dept);
        });

        group.MapPost("/", async (DepartmentDto dto, IDepartmentService service, IValidator<DepartmentDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var newDept = await service.CreateAsync(dto);
            return Results.Created($"/api/departments/{newDept.Id}", newDept);
        });

        group.MapPut("/{id:Guid}", async (Guid id, DepartmentDto dto, IDepartmentService service, IValidator<DepartmentDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var updated = await service.UpdateAsync(id, dto);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });


        group.MapDelete("/{id:Guid}", async (Guid id, IDepartmentService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}