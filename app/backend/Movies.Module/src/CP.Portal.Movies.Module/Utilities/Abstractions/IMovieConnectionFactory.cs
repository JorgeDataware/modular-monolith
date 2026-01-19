using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CP.Portal.Movies.Module.Utilities.Abstractions;

internal interface IMovieConnectionFactory
{
    IDbConnection CreateConnection();
}

internal class MovieConnectionFactory(IConfiguration configuration) : IMovieConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString("MoviesConnectionString");
        return new SqlConnection(connectionString);
    }
}