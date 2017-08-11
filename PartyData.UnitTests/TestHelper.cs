using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PartyData.UnitTests
{
    class TestHelper
    {
        internal static DbContextOptions<PartyDbContext> GetInMemoryDbContextOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<PartyDbContext>()
                    .UseSqlite(connection)
                    .Options;

            return options;
        }
    }
}
