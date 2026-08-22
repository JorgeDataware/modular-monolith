using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.GetListGenresAsync;

internal class ListGenresEndpoint(IGenreService genreService) : EndpointWithoutRequest<ApiResponse<IEnumerable<GenreDto>>>
{
    private readonly IGenreService _genreServcie = genreService;

    public override void Configure()
    {
        Get("/api/Genre/all");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _genreServcie.ListGenresAsync(ct);

        await this.SendApiResponseAsync(result, "Géneros obtenidos correctamente", ct: ct);
    }
}
