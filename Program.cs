using Company_ManagementAPI.Data;
using Company_ManagementAPI.Endpoints;
using Company_ManagementAPI.Services;
using Company_ManagementAPI.Validators;
// using Company_ManagementAPI.Validators;
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
builder.Services.AddScoped<IRandomStringGeneratorService, RandomStringGeneratorService>();
// builder.Services.AddFluentValidationAutoValidation(); // Optional but useful
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeDtoValidator>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEmployeeEndpoints();
app.MapProjectEndpoints();
app.MapDepartmentEndpoints();

// app.UseHttpsRedirection();

app.Run();
