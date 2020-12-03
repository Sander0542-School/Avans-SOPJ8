using Bumbo.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bumbo.Tests.Utils
{
    public class TestDatabaseContextFactory
    {
        public static ApplicationDbContext CreateDbContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug().AddConsole());

            var connectionStringBuilder = new SqliteConnectionStringBuilder { Mode = SqliteOpenMode.Memory };
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            contextOptionsBuilder.EnableSensitiveDataLogging();
            contextOptionsBuilder.UseLoggerFactory(loggerFactory);
            contextOptionsBuilder.UseSqlite(connection);

            var context = new ApplicationDbContext(contextOptionsBuilder.Options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}