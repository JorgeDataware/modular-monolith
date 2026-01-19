namespace CP.Portal.Movies.Module.Domain.Repositories;

internal interface IReadOnlyRepository
{
    Task<Movie?> GetByIdAsync(Guid Id, CancellationToken ct);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
}
