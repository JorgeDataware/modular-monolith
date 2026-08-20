using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.DeleteGenre;

internal class DeleteGenreEndpoint(IGenreService genreService)
    : EndpointWithoutRequest<ApiResponse<string>>
{
    private readonly IGenreService _genreService = genreService;

    public override void Configure()
    {
        Delete("/api/Genre/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");

        var result = await _genreService.DeleteGenreAsync(id, ct);

        await this.SendApiResponseAsync(result, "Género eliminado exitosamente", ct: ct);
    }
}
