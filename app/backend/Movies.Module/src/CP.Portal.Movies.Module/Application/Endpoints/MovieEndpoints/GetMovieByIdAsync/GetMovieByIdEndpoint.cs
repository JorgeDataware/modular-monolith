using CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.GetMovieByIdAsync;

internal class GetMovieByIdEndpoint(IMovieService movieService) : EndpointWithoutRequest<GetMovieDetailByIdDto>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/Movie/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");

        var result = await _movieService.GetMovieByIdAsync(id, ct);

        await this.SendApiResponseAsync(result, "Película obtenida correctamente", ct: ct);
    }
}
