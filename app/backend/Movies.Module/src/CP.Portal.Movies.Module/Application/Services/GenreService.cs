using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.CreateGenre;
using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.GetListGenresAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories.GenreRepository;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using FluentValidation;

namespace CP.Portal.Movies.Module.Application.Services;

internal class GenreService(IGenreRepository genreRepository, IValidator<AddGenreRequest> validator, IMapper mapper) : IGenreService
{
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IValidator<AddGenreRequest> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<string>> CreateGenreAsync(AddGenreRequest request, CancellationToken ct)
    {
        var val = await _validator.ValidateAsync(request, ct);
        if (!val.IsValid)
        {
            var errors = string.Join("; ", val.Errors.Select(e => e.ErrorMessage));
            return Result<string>.Failure(new Error("ValidationError", errors));
        }

        var genre = _mapper.Map<Genre>(request);

        _genreRepository.Add(genre);
        return Result<string>.Success(genre.Id.ToString());
    }

    public async Task<Result<string>> DeleteGenreAsync(Guid genreId, CancellationToken ct)
    {
        await _genreRepository.Delete(genreId, ct);
        return Result<string>.Success("Genre deleted successfully");
    }

    public async Task<Result<IEnumerable<GenreDto>>> ListGenresAsync(CancellationToken ct)
    {
        var genres = await _genreRepository.GetAllAsync(ct);
        
        var genreDtos = _mapper.Map<IEnumerable<GenreDto>>(genres);

        return Result<IEnumerable<GenreDto>>.Success(genreDtos);
    }
}
