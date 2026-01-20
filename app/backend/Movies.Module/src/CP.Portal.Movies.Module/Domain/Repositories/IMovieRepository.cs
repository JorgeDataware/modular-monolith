using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Domain.Repositories;

internal interface IMovieRepository : IReadOnlyRepository
{
    void Add(Movie movie);
    Task Delete(Guid Id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
