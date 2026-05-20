using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal class MovieRepository(MovieDbContext dbContext) : IMovieRepository
{
    private readonly MovieDbContext _dbContext = dbContext;

    public async Task AddAsync(Movie movie, CancellationToken ct)
        => await _dbContext.movies.AddAsync(movie);

    public async Task<int> DeleteAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.Where(m => m.Id == id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
        => await _dbContext.movies.AsNoTracking().ToListAsync(ct);

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}