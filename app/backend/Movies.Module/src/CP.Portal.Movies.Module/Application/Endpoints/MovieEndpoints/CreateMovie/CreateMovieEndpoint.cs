using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;

internal class CreateMovieEndpoint(IMovieService movieService) : Endpoint<AddMovieRequest, ApiResponse<Guid>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Post("/api/Movie");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddMovieRequest request, CancellationToken ct)
    {
        var result = await _movieService.CreateMovieAsync(request, ct);

        await this.SendApiResponseAsync(result, "Película añadida correctamente.", 201, ct);
    }
}
