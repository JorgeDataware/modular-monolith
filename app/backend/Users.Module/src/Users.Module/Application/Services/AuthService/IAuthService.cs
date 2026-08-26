using Core.Contracts.Abstractions;
using Users.Module.Application.Endpoints.AuthEndpoints.Login;

namespace Users.Module.Application.Services.Auth;

internal interface IAuthService
{
    Task<Result<string>> LoginAsync(LoginRequest request, CancellationToken ct);
}
