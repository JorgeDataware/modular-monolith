using CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;

namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken ct);
    Task<int> DeleteAsync(Guid Id, CancellationToken ct);
    Task<Movie?> GetByIdAsync(Guid Id, CancellationToken ct);
    Task<Movie?> GetMovieAsync(Guid Id, CancellationToken ct);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<GetMovieDetailByIdDto?> GetMovieDetailAsync(Guid Id, CancellationToken ct);
}
