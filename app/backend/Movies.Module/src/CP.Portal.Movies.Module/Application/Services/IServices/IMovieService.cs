using CP.Portal.Movies.Module.Application.GetListMovies;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetMovies();
}
