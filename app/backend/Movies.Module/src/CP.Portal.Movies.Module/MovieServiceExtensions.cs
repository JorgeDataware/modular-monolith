using CP.Portal.Movies.Module.Application.Services;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain.Repositories.Movie;
using CP.Portal.Movies.Module.Infrastructure;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Validators.Movie;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CP.Portal.Movies.Module;

public static class MovieServiceExtensions
{
    public static IServiceCollection MovieService (this IServiceCollection services, ConfigurationManager config)
    {
        // Inyección de servicios
        services.AddScoped<IMovieService, MovieService>();

        // Inyección de repositorios
        services.AddScoped<IMovieRepository, MovieRepository>();

        // Inyección de MovieDbContext en el contenedor de servicios
        string? connectionString = config.GetConnectionString("MoviesConnectionString");
        services.AddDbContext<MovieDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        // Inyección de FluentValidation
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssemblyContaining<MovieInsertValidator>();

        // REGISTRO DE AUTOMAPPER
        services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });

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
