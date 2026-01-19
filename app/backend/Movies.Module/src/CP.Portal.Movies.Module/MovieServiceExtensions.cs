using CP.Portal.Movies.Module.Application.GetListMovies;
using CP.Portal.Movies.Module.Domain.Repositories;
using CP.Portal.Movies.Module.Infrastructure;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using Microsoft.AspNetCore.Builder;
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

        // Inyección de repositorios
        services.AddScoped<IMovieRepository, MovieRepository>();

        // Inyección de MovieDbContext en el contenedor de servicios
        string? connectionString = config.GetConnectionString("MoviesConnectionString");
        services.AddDbContext<MovieDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        // Inyección de la fábrica de conexiones para Dapper
        services.AddScoped<IMovieConnectionFactory, MovieConnectionFactory>();

        return services;
    }

    public static IApplicationBuilder UseMoviesModuleMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            // Aquí dentro SÍ podemos ver MovieDbContext porque estamos dentro del módulo
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
            dbContext.Database.Migrate();
        }
        return app;
    }
}
