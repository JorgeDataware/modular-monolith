using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;

internal class GetListMoviesEndpoint(IMovieService movieService)
    : EndpointWithoutRequest<ApiResponse<IEnumerable<MovieDto>>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/movies/all");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _movieService.ListMovieAsync(ct);

        await this.SendApiResponseAsync(result, "Películas obtenidas exitosamente", ct: ct);
    }
}
