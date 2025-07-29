using Company_ManagementAPI.DTO;
using Company_ManagementAPI.Services;

namespace Company_ManagementAPI.Endpoints;

public static class ProjectEndpoint
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects");

        group.MapGet("/", async (IProjectService service) =>
        {
            var projects = await service.GetAllAsync();
            return Results.Ok(projects);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProjectService service) =>
        {
            var project = await service.GetByIdAsync(id);
            return project is not null ? Results.Ok(project) : Results.NotFound();
        });

        group.MapPost("/", async (ProjectDto dto, IProjectService service) =>
        {
            var project = await service.CreateAsync(dto);
            return Results.Created($"/api/projects/{project.Id}", project);
        });

        group.MapPut("/{id:guid}", async (Guid id, ProjectDto dto, IProjectService service) =>
        {
            var updated = await service.UpdateAsync(id, dto);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProjectService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}