using Core.Contracts.Abstractions;
using FastEndpoints;
using Users.Module.Application.Services.CartMovieService;

namespace Users.Module.Application.Endpoints.CartMovieEndpoints.DeleteCartMovie;

internal class DeleteCartMovieEndpoint(ICartMovieService cartService) : EndpointWithoutRequest<ApiResponse<Guid>>
{
    private readonly ICartMovieService _cartService = cartService;

    public override void Configure()
    {
        Delete("api/CartMovie/{MovieId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var movieId = Route<Guid>("MovieId");
    }
}
