using Core.Contracts.Abstractions;
using CP.Portal.Movies.Module.Domain.Repositories.MovieRepository;
using MediatR;
using Movies.Module.Contracts.Dtos;
using Movies.Module.Contracts.Errors;
using Movies.Module.Contracts.Queries;

namespace CP.Portal.Movies.Module.Application.Integrations;

/// <summary>
/// Atiende <see cref="GetMovieSummaryQuery"/>: es la fachada del módulo hacia el exterior.
///
/// Vive en el módulo de implementación (no en el de contratos) y es internal:
/// nadie fuera puede instanciarlo, solo MediatR lo resuelve por DI. Internamente
/// reutiliza el repositorio que ya existe, sin duplicar lógica.
/// </summary>
internal sealed class GetMovieSummaryQueryHandler(IMovieRepository movieRepository)
    : IRequestHandler<GetMovieSummaryQuery, Result<MovieSummary>>
{
    public async Task<Result<MovieSummary>> Handle(GetMovieSummaryQuery request, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetMovieAsync(request.MovieId, cancellationToken);

        if (movie is null)
            return Result<MovieSummary>.Failure(MoviesContractErrors.MovieNotFound);

        // Se traduce la entidad interna al DTO del contrato. La entidad nunca cruza el límite.
        return Result<MovieSummary>.Success(new MovieSummary(movie.Id, movie.Title, movie.RentalPrice));
    }
}
