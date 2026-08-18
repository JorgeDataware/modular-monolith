using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;

internal class GenreRepository(MovieDbContext dbContext) : IGenreRepository
{
    private readonly MovieDbContext _dbContext = dbContext;

    public void Add(Genre genre)
        => _dbContext.genres.Add(genre);

    public async Task Delete(Guid Id, CancellationToken ct)
        => await _dbContext.genres.Where(m => m.Id == Id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken ct)
        => await _dbContext.genres.ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}
