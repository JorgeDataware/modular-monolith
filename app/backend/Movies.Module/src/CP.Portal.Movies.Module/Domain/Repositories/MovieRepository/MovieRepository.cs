using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal class MovieRepository(MovieDbContext dbContext) : IMovieRepository
{
    private readonly MovieDbContext _dbContext = dbContext;

    public void Add(Movie movie)
        => _dbContext.movies.Add(movie);

    public async Task Delete(Guid id, CancellationToken ct)
        => await _dbContext.movies.Where(m => m.Id == id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
        => await _dbContext.movies.AsNoTracking().ToListAsync(ct);

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.FindAsync(id, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}