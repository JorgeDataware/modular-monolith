using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.CreateGenre;
using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.GetListGenresAsync;
using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IGenreService
{
    Task<Result<IEnumerable<GenreDto>>> ListGenresAsync(CancellationToken ct);
    Task<Result<string>> CreateGenreAsync(AddGenreRequest request, CancellationToken ct);
    Task<Result<string>> DeleteGenreAsync(Guid genreId, CancellationToken ct);
}
