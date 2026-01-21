using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;
using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IMovieService
{
    Task<Result<ListMoviesDto>> ListMovieAsync(CancellationToken ct);
    Task<Result<MovieDto>> GetMovieByIdAsync(Guid id, CancellationToken ct);
    Task<Result<string>> CreateMovieAsync(AddMovieRequest request, CancellationToken ct);
    Task<Result<string>> DeleteMovieAsync(Guid id, CancellationToken ct);
}
