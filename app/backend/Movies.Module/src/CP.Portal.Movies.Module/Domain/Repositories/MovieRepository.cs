using CP.Portal.Movies.Module.Infrastructure;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories;

internal class MovieRepository(MovieDbContext dbContext, IMovieConnectionFactory connectionFactory) : IMovieRepository
{
    private readonly MovieDbContext _dbContext = dbContext;
    private readonly IMovieConnectionFactory _connectionFactory = connectionFactory;

    public async Task<Result<string>> AddAsync(Movie movie)
    {
        try
        {
            _dbContext.Add(movie);
            return Result<string>.Success("Movie added successfully");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new Error("MOVIE_ADD_ERROR", ex.Message));
        }
    }

    public async Task<Result<string>> DeleteAsync(Guid Id, CancellationToken ct)
    {
        try
        {
            await _dbContext.movies.
                Where(m => m.Id == Id)
                .ExecuteDeleteAsync(ct);
            return Result<string>.Success("Movie deleted successfully");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new Error("MOVIE_DELETE_ERROR", ex.Message));
        }
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
        => await _dbContext.movies.ToListAsync(ct);

    public async Task<Movie?> GetByIdAsync(Guid Id, CancellationToken ct)
        => await _dbContext.movies.FindAsync(Id, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}
