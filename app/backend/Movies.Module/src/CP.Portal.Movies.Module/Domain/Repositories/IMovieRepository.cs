using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Domain.Repositories;

internal interface IMovieRepository : IReadOnlyRepository
{
    Task<Result<string>> AddAsync(Movie movie);
    Task<Result<string>> DeleteAsync(Guid Id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
