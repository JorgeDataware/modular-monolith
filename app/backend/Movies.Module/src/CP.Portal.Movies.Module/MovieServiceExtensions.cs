using CP.Portal.Movies.Module.Application.GetListMovies;
using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Portal.Movies.Module;

public static class MovieServiceExtensions
{
    public static IServiceCollection MovieService (this IServiceCollection services, ConfigurationManager config)
    {
        // Inyecciones de servicios específicos (verticales)
        services.AddScoped<GetListMoviesService>();

        // Inyección de MovieDbContext en el contenedor de servicios
        string? connectionString = config.GetConnectionString("MoviesConnectionString");
        services.AddDbContext<MovieDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
        return services;
    }
}
