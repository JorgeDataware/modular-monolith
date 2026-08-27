using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;
using Users.Module.Application.Services.CartMovieService;
using Users.Module.Utilities.Extensions;

namespace Users.Module.Application.Endpoints.CartMovieEndpoints.AddCartMovie;

internal class AddCartMovieEndpoint(ICartMovieService cartService) : EndpointWithoutRequest<ApiResponse<Guid>>
{
    private readonly ICartMovieService _cartService = cartService;

    public override void Configure()
    {
        Post("api/CartMovie/{MovieId}");
        AuthSchemes("Bearer");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!User.TryGetUserId(out var userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var movieId = Route<Guid>("MovieId");

        var result = await _cartService.AddCartMovieAsync(movieId, userId, ct);

        await this.SendApiResponseAsync(result, "Película agregada al carrito", 201, ct);
    }
}
