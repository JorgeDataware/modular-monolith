using Core.Contracts.Abstractions;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Users.Module.Application.Endpoints.AuthEndpoints.Login;
using Users.Module.Domain;
using Users.Module.Utilities.Configuration;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.Auth;

internal class AuthService(UserManager<User> userManager, IOptions<JWTConfigs> jwtOptions) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JWTConfigs _jwtOptions = jwtOptions.Value;

    public async Task<Result<string>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email).WaitAsync(ct);

        if (user is null)
            return Result<string>.Failure(UserErrors.IvalidCredentials);

        if (!(await _userManager.CheckPasswordAsync(user, request.Password)))
            return Result<string>.Failure(UserErrors.IvalidCredentials);

        var secret = _jwtOptions.Secret;

        var token = JwtBearer.CreateToken(option =>
        {
            option.SigningKey = secret;
            option.ExpireAt = DateTime.UtcNow.AddHours(500);
            option.User["sub"] = user.Id;
            option.User["email"] = user.Email;
            option.User["name"] = user.FullName;
        });

        return Result<string>.Success(token);
    }
}
