using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace VideoLibrary.DataContext
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("ConnectionStrings-VideoLibrary-DB");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
