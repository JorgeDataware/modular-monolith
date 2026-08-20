using CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;
using CP.Portal.Movies.Module.Infrastructure;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;

internal class MovieRepository(MovieDbContext dbContext, IMovieConnectionFactory connectionFactory) : IMovieRepository
{
    private readonly MovieDbContext _dbContext = dbContext;
    private readonly IMovieConnectionFactory _connectionFactory = connectionFactory;

    public async Task AddAsync(Movie movie, CancellationToken ct)
        => await _dbContext.movies.AddAsync(movie, ct);

    public async Task<int> DeleteAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.Where(m => m.Id == id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
        => await _dbContext.movies.AsNoTracking().ToListAsync(ct);

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<Movie?> GetMovieAsync(Guid id, CancellationToken ct)
        => await _dbContext.movies.FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<GetMovieDetailByIdDto?> GetMovieDetailAsync(Guid Id, CancellationToken ct)
    {
        const string sql = """
            -- Información de la película
            SELECT
                m.Id,
                m.Title,
                m.Description,
                m.ReleaseYear,
                m.DurationMinutes,
                m.Language,
                m.RentalPrice,
                m.CreatedAt
            FROM movies.movies m
            WHERE m.Id = @movieId;

            -- Cast
            SELECT
                c.PersonId AS PersonId,
                CONCAT(p.FirstName, ' ', p.LastName) AS FullName,
                c.Character AS Role
            FROM movies.casters c
            JOIN movies.persons p ON p.Id = c.PersonId
            WHERE c.MovieId = @movieId;

            -- Crew
            SELECT
                c.PersonId AS PersonId,
                CONCAT(p.FirstName, ' ', p.LastName) AS FullName,
                c.Role AS Role
            FROM movies.crewers c
            JOIN movies.persons p ON p.Id = c.PersonId
            WHERE c.MovieId = @movieId;

            -- Géneros
            SELECT
                g.Id AS GenreId,
                g.Name
            FROM movies.movie_genres mg
            JOIN movies.genres g ON g.Id = mg.GenreId
            WHERE mg.MovieId = @movieId;
            """;

        using var connection = await _connectionFactory.CreateConnection();

        var command = new CommandDefinition(sql, new { movieId = Id }, cancellationToken: ct);

        using var multi = await connection.QueryMultipleAsync(command);

        // Los grids se deben leer en el mismo orden en que se declararon en el SQL.
        var movie = await multi.ReadSingleOrDefaultAsync<MovieDetailRow>();

        if (movie is null)
            return null;

        var cast = (await multi.ReadAsync<ParticipanDto>()).ToList();
        var crew = (await multi.ReadAsync<ParticipanDto>()).ToList();
        var genres = (await multi.ReadAsync<GenreDto>()).ToList();

        return new GetMovieDetailByIdDto(
            movie.Id,
            movie.Title,
            movie.Description,
            DateOnly.FromDateTime(movie.ReleaseYear),
            movie.DurationMinutes,
            movie.Language,
            movie.RentalPrice,
            movie.CreatedAt,
            cast,
            crew,
            genres
        );
    }

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}