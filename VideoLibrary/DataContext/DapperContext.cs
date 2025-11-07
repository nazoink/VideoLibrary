using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace VideoLibrary.DataContext
{
    /// <summary>
    /// Helper class that provides IDbConnection instances configured from IConfiguration.
    /// </summary>
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Constructs the context and reads the connection string from configuration using key "ConnectionStrings-VideoLibrary-DB".
        /// Consider using the standard "ConnectionStrings:VideoLibrary-DB" key or configuring via secrets for production.
        /// </summary>
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("ConnectionStrings-VideoLibrary-DB");
        }

        /// <summary>
        /// Creates a new <see cref="IDbConnection"/> (SqlConnection) using the configured connection string.
        /// Caller is responsible for disposing the returned connection (e.g., using statement).
        /// </summary>
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
