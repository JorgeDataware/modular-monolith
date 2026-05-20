namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal interface IMovieRepository
{
    Task AddAsync(Movie movie);
    Task DeleteAsync(Guid Id, CancellationToken ct);
    Task<Movie?> GetByIdAsync(Guid Id, CancellationToken ct);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
