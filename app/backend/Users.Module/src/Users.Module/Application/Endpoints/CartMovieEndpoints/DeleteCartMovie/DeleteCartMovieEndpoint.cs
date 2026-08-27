using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;
using Users.Module.Application.Services.CartMovieService;
using Users.Module.Utilities.Extensions;

namespace Users.Module.Application.Endpoints.CartMovieEndpoints.DeleteCartMovie;

internal class DeleteCartMovieEndpoint(ICartMovieService cartService) : EndpointWithoutRequest<ApiResponse<Guid>>
{
    private readonly ICartMovieService _cartService = cartService;

    public override void Configure()
    {
        Delete("api/CartMovie/{MovieId}");
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

        var result = await _cartService.DeleteCartMovieAsync(movieId, userId, ct);

        await this.SendApiResponseAsync(result, "Película eliminada del carrito", ct: ct);
    }
}
