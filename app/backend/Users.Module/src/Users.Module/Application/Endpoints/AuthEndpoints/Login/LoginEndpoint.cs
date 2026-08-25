using Core.Contracts.Extensions;
using FastEndpoints;
using Users.Module.Application.Services.Auth;

namespace Users.Module.Application.Endpoints.AuthEndpoints.Login;

internal class LoginEndpoint(IAuthService authService) : Endpoint<LoginRequest>
{
    private readonly IAuthService _authService = authService;
    public override void Configure()
    {
        Post("/api/Auth/Login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);

        await this.SendApiResponseAsync(result, "Login exitoso", 200, ct);
    }
}
