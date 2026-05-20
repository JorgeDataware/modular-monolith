using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Errors;
using FluentValidation;

namespace CP.Portal.Movies.Module.Application.Services;

internal class MovieService(IMovieRepository movieRepository, IValidator<AddMovieRequest> validator, IMapper mapper) : IMovieService
{
    private readonly IMovieRepository _movieRepository = movieRepository;
    private readonly IValidator<AddMovieRequest> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<string>> CreateMovieAsync(AddMovieRequest request, CancellationToken ct)
    {
        var val = _validator.Validate(request);
        if (!val.IsValid)
        {
            var error = val.Errors.FirstOrDefault();
            return Result<string>.Failure(new Error("CreateMovieError", error.ErrorMessage!));
        }

        var movie = _mapper.Map<Movie>(request);
        await _movieRepository.AddAsync(movie, ct);
        await _movieRepository.SaveChangesAsync(ct);

        return Result<string>.Success(movie.Id.ToString());
    }

    public async Task<Result<string>> DeleteMovieAsync(Guid id, CancellationToken ct)
    {
        var rows = await _movieRepository.DeleteAsync(id, ct);

        if (rows < 1)
            return Result<string>.Failure(MovieErrors.MovieNotFound);

        return Result<string>.Success("The movie was successfully deleted");
    }

    public async Task<Result<MovieDto>> GetMovieByIdAsync(Guid id, CancellationToken ct)
    {
        var movie = await _movieRepository.GetByIdAsync(id, ct);

        if (movie == null)
            return Result<MovieDto>.Failure(MovieErrors.MovieNotFound);

        return Result<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
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

    public Task<Result<Guid>> UpdateMoviePrice(Guid id, decimal price)
    {
        throw new NotImplementedException();
    }
}
