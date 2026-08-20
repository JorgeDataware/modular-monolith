using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.UpdateMoviePriceAsync;

internal class UpdateMoviePriceEndpoint(IMovieService movieService)
    : Endpoint<UpdateMoviePriceRequest, ApiResponse<Guid>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Patch("/api/Movie/{Id}/price");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateMoviePriceRequest request, CancellationToken ct)
    {
        var result = await _movieService.UpdateMoviePrice(request.Id, request.RentalPrice, ct);

        await this.SendApiResponseAsync(result, "Precio actualizado exitosamente", ct: ct);
    }
}
