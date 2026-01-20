using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Infrastructure.Repositories;

internal class MovieRepository(MovieDbContext dbContext) : IMovieRepository
{
    public void Add(Movie movie)
    {
        dbContext.movies.Add(movie);
    }

    public void Delete(Movie movie)
    {
        dbContext.movies.Remove(movie);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        await dbContext.movies
           .Where(m => m.Id == id)
           .ExecuteDeleteAsync(ct);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
    {
        return await dbContext.movies.ToListAsync(ct);
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.movies.FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}