using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;
using CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;
using Core.Contracts.Abstractions;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IMovieService
{
    Task<Result<IEnumerable<MovieDto>>> ListMovieAsync(CancellationToken ct);
    Task<Result<GetMovieDetailByIdDto>> GetMovieByIdAsync(Guid id, CancellationToken ct);
    Task<Result<string>> CreateMovieAsync(AddMovieRequest request, CancellationToken ct);
    Task<Result<string>> DeleteMovieAsync(Guid id, CancellationToken ct);
    Task<Result<Guid>> UpdateMoviePrice(Guid id, decimal price, CancellationToken ct);
}
