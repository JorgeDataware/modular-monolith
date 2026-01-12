using CP.Portal.Movies.Module.Application.GetListMovies;
using CP.Portal.Movies.Module.Application.Services.IServices;

namespace CP.Portal.Movies.Module.Application.Services;

internal class MovieService : IMovieService
{
    public async Task<IEnumerable<MovieDto>> GetMovies()
    {
        // In a real application, this data might come from a database
        var movies = new List<MovieDto>
        {
            new MovieDto(Guid.NewGuid(), "Inception", "A mind-bending thriller by Christopher Nolan."),
            new MovieDto(Guid.NewGuid(), "The Matrix", "A sci-fi classic that questions reality."),
            new MovieDto(Guid.NewGuid(), "Interstellar", "A journey through space and time.")
        };
        return movies;
    }
}
