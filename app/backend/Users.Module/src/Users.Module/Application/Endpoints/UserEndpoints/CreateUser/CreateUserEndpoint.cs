using Core.Contracts.Abstractions;
using FastEndpoints;
using Users.Module.Application.Services.UserService;

namespace Users.Module.Application.Endpoints.UserEndpoints.CreateUser;

internal class CreateUserEndpoint(IUserService userService) : Endpoint<CreateUserRequest, ApiResponse<Guid>>
{
    private readonly IUserService _userService = userService;

    public override void Configure()
    {
        Post("/api/Users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
    {

    }
}
