using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;

internal class GetListMoviesEndpoint(IMovieService movieService) : EndpointWithoutRequest<IEnumerable<MovieDto>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/movies/GetMovies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct = default)
    {
        var result = await _movieService.ListMovieAsync(ct);

        if (result.IsSuccess)
        {
            await this.SendStandardSuccessAsync(
                result.Value.Movies,
                "Películas obtenidas exitosamente",
                statusCode: 200,
                ct: ct
            );
        }
        else
        {
            await this.SendStandardFailureAsync<MovieDto>(
                error: result.Error.Code,
                message: result.Error.Message,
                statusCode: 400,
                ct: ct
            );
        }
    }
}
