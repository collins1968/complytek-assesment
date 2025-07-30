using CompanyManagementAPI.Data;
using CompanyManagementAPI.Endpoints;
using CompanyManagementAPI.Extensions;
using CompanyManagementAPI.Services;
using CompanyManagementAPI.Validators;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddHttpClient<IRandomStringGeneratorService, RandomStringGeneratorService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeDtoValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapEmployeeEndpoints();
app.MapProjectEndpoints();
app.MapDepartmentEndpoints();

// app.UseHttpsRedirection();

app.Run();
