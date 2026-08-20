namespace CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;

internal interface IGenreRepository
{
    Task Add(Genre genre, CancellationToken ct);
    Task Delete(Guid Id, CancellationToken ct);
    Task<IEnumerable<Genre>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
