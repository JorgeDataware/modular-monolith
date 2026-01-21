namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal interface IMovieRepository
{
    void Add(Movie movie);
    Task Delete(Guid Id, CancellationToken ct);
    Task<Movie?> GetByIdAsync(Guid Id, CancellationToken ct);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
