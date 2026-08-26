using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;
using Users.Module.Application.Services.UserService;
using Users.Module.Utilities.Extensions;

namespace Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;

internal class GetMyUserEndpoint(IUserService userService) : EndpointWithoutRequest<ApiResponse<GetUserDto>>
{
    private readonly IUserService _userService = userService;

    public override void Configure()
    {
        Get("/api/Users/Me");
        AuthSchemes("Bearer");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!User.TryGetUserId(out var userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await _userService.GetUserByIdAsync(userId, ct);

        await this.SendApiResponseAsync(result, "Usuario encontrado", ct: ct);
    }
}
