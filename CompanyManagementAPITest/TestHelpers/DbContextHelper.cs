using Microsoft.EntityFrameworkCore;
using CompanyManagementAPI.Data;
using Microsoft.Data.Sqlite;

namespace CompanyManagementAPITest.TestHelpers;

public static class DbContextHelper
{
    public static AppDbContext GetInMemoryDbContext()
    {
        // Use SQLite in-memory database which supports transactions
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
    public static AppDbContext GetInMemoryDbContext1()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}