using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using sdev2301_a1_JoelBadege.Data;


namespace sdev2301_a1_JoelBadege.Tests
{
    public static class DbTestHelper
    {
        public static SqliteConnection CreateOpenInMemoryConnection()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public static DbContextOptions<AppDbContext> CreateOptions(SqliteConnection connection)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;
        }

        public static AppDbContext CreateContext(DbContextOptions<AppDbContext> options, bool ensureCreated = false)
        {
            var context = new AppDbContext(options);

            if (ensureCreated)
                context.Database.EnsureCreated();

            return context;
        }
    }
}