using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.DeleteMovie;

internal class DeleteMovieEndpoint(IMovieService movieService) : EndpointWithoutRequest<ApiResponse<Guid>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Delete("api/Movie/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync (CancellationToken ct)
    {
        var id = Route<Guid>("Id");
        var result = await _movieService.DeleteMovieAsync(id, ct);

        await this.SendApiResponseAsync(result, "Película eliminada correctamente", ct: ct);
    }
}