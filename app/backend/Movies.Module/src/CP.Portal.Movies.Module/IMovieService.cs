using CP.Portal.Movies.Module.Application.GetListMovies;

namespace CP.Portal.Movies.Module;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetMovies();
}
