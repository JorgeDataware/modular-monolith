using CP.Portal.Movies.Module.Application.GetListMovies;
using CP.Portal.Movies.Module.Application.Services;
using CP.Portal.Movies.Module.Application.Services.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Portal.Movies.Module;

public static class MovieServiceExtensions
{
    public static IServiceCollection MovieService (this IServiceCollection services)
    {
        // Inyección de un servicio general (horizontal)
        services.AddScoped<IMovieService, MovieService>();
        // Inyecciones de servicios específicos (verticales)
        services.AddScoped<GetListMoviesService>();
        return services;
    }
}
