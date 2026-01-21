namespace CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;

internal interface IGenreRepository
{
    void Add(Genre genre);
    Task Delete(Guid Id, CancellationToken ct);
    Task<IEnumerable<Genre>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
