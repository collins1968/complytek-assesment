using CompanyManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementAPI.Extensions;

public static class MigrationExtension
{
      public static void ApplyMigrations(this IApplicationBuilder app)
      {
          using var serviceScope = app.ApplicationServices.CreateScope();
          using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
          context.Database.Migrate();
      }
}