using Npgsql;
using System.Data;

namespace CineApi.Context
{
    public class AppDbContext
    {
        private readonly string dbConnectionString;
        public AppDbContext(IConfiguration configuration)
        {
            dbConnectionString = configuration.GetConnectionString("MovieDb");
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(dbConnectionString);
    }
}
