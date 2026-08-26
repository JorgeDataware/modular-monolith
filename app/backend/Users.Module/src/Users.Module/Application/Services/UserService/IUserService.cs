using Core.Contracts.Abstractions;
using Users.Module.Application.Endpoints.UserEndpoints.CreateUser;
using Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;

namespace Users.Module.Application.Services.UserService;

internal interface IUserService
{
    Task<Result<GetUserDto>> GetUserByIdAsync(string Id, CancellationToken ct);
    Task<Result<string>> CreateUserAsync(CreateUserRequest request, CancellationToken ct);
}
