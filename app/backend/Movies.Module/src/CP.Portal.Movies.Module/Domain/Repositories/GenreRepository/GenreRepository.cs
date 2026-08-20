using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;

internal class GenreRepository(MovieDbContext dbContext) : IGenreRepository
{
    private readonly MovieDbContext _dbContext = dbContext;

    public async Task Add(Genre genre, CancellationToken ct)
        => await _dbContext.genres.AddAsync(genre, ct);

    public async Task Delete(Guid Id, CancellationToken ct)
        => await _dbContext.genres.Where(m => m.Id == Id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken ct)
        => await _dbContext.genres.ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}
