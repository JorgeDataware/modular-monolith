using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;
using CP.Portal.Movies.Module.Utilities.Abstractions;
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
            var errors = string.Join("; ", val.Errors.Select(e => e.ErrorMessage));
            return Result<string>.Failure(new Error("ValidationError", errors));
        }

        var movie = _mapper.Map<Movie>(request);
        _movieRepository.Add(movie);


    }

    public Task<Result<string>> DeleteMovieAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result<MovieDto>> GetMovieByIdAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ListMoviesDto>> ListMovieAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
