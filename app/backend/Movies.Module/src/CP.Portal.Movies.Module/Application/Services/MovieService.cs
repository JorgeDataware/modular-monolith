using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;
using CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Errors;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FluentValidation;

namespace CP.Portal.Movies.Module.Application.Services;

internal class MovieService(IMovieRepository movieRepository, IValidator<AddMovieRequest> validator, IMapper mapper) : IMovieService
{
    private readonly IMovieRepository _movieRepository = movieRepository;
    private readonly IValidator<AddMovieRequest> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<string>> CreateMovieAsync(AddMovieRequest request, CancellationToken ct)
    {
        var val = await _validator.ValidateAsync(request, ct);
        if (!val.IsValid)
            return val.ToFailure<string>();

        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            DurationMinutes = request.DurationMinutes,
            Language = request.Language,
            RentalPrice = request.RentalPrice
        };

        var casters = request.Casters.Select(c => new Cast
        {
            MovieId = movie.Id,
            PersonId = c.PersonId,
            Character = c.role
        }).ToList();

        var crewers = request.Crewers.Select(c => new Crew
        {
            MovieId = movie.Id,
            PersonId = c.PersonId,
            Role = c.role
        }).ToList();

        var genres = request.Genres.Select(g => new MovieGenre
        {
            MovieId = movie.Id,
            GenreId = g
        }).ToList();

        movie.Casts = casters;
        movie.Crewers = crewers;
        movie.MovieGenres = genres;

        await _movieRepository.AddAsync(movie, ct);
        await _movieRepository.SaveChangesAsync(ct);

        return Result<string>.Success(movie.Id.ToString());
    }

    public async Task<Result<string>> DeleteMovieAsync(Guid id, CancellationToken ct)
    {
        var rows = await _movieRepository.DeleteAsync(id, ct);

        if (rows < 1)
            return Result<string>.Failure(MovieErrors.MovieNotFound);

        await _movieRepository.SaveChangesAsync(ct);

        return Result<string>.Success("The movie was successfully deleted");
    }

    public async Task<Result<GetMovieDetailByIdDto>> GetMovieByIdAsync(Guid id, CancellationToken ct)
    {
        var movie = await _movieRepository.GetMovieDetailAsync(id, ct);

        if (movie is null)
            return Result<GetMovieDetailByIdDto>.Failure(MovieErrors.MovieNotFound);

        return Result<GetMovieDetailByIdDto>.Success(movie);
    }

    public async Task<Result<IEnumerable<MovieDto>>> ListMovieAsync(CancellationToken ct)
    {
        var rawMovies = await _movieRepository.GetAllAsync(ct);

        IEnumerable<MovieDto> result = rawMovies.Select(m => new MovieDto
        (
            m.Id,
            m.Title,
            m.Description
        ));

        return Result<IEnumerable<MovieDto>>.Success(result);
    }

    public async Task<Result<Guid>> UpdateMoviePrice(Guid id, decimal price, CancellationToken ct)
    {
        var movie = await _movieRepository.GetMovieAsync(id, ct);

        if (movie == null)
            return Result<Guid>.Failure(MovieErrors.MovieNotFound);

        if (movie.RentalPrice == price)
            return Result<Guid>.Success(id);

        movie.RentalPrice = price;
        await _movieRepository.SaveChangesAsync(ct);

        return Result<Guid>.Success(id);
    }
}   
