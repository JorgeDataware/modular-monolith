using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CP.Portal.Movies.Module;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints (this WebApplication app)
    {
        app.MapGet("/GetMovies", (IMovieService movieService) => { 
            var movies = movieService.GetMovies();
            return Results.Ok(movies);
        });
    }
}
