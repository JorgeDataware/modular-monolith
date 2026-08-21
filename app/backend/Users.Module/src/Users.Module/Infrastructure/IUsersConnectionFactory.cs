using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Users.Module.Infrastructure;

internal interface IUsersConnectionFactory
{
    Task<IDbConnection> CreateConnection();
}

internal class UsersConnectionFactory(IConfiguration configuration) : IUsersConnectionFactory
{
    public async Task<IDbConnection> CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString("MoviesConnectionString");
        return new SqlConnection(connectionString);
    }
}