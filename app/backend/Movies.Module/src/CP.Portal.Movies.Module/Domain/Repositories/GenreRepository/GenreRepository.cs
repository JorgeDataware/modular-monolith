using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;

internal class GenreRepository(MovieDbContext dbContext) : IGenreRepository
{
    public void Add(Genre genre)
        => dbContext.genres.Add(genre);

    public async Task Delete(Guid Id, CancellationToken ct)
        => await dbContext.genres.Where(m => m.Id == Id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken ct)
        => await dbContext.genres.ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await dbContext.SaveChangesAsync(ct);
}
