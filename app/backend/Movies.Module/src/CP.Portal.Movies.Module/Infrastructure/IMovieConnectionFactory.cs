using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CP.Portal.Movies.Module.Infrastructure;

internal interface IMovieConnectionFactory
{
    Task<IDbConnection> CreateConnection();
}

internal class MovieConnectionFactory(IConfiguration configuration) : IMovieConnectionFactory
{
    public async Task<IDbConnection> CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString("MoviesConnectionString");
        return new SqlConnection(connectionString);
    }
}