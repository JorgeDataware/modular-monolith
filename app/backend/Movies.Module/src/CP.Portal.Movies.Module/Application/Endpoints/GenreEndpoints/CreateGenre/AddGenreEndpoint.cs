using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.CreateGenre;

internal class AddGenreEndpoint(IGenreService genreService) : Endpoint<AddGenreRequest, ApiResponse<Guid>>
{
    private readonly IGenreService _genreService = genreService;


    public override void Configure()
    {
        Post("/api/Genre");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddGenreRequest request, CancellationToken ct)
    {
        var result = await _genreService.CreateGenreAsync(request, ct);

        await this.SendApiResponseAsync(result, "Género añadido correctamente", ct: ct);
    }
}
