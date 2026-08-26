namespace Users.Module.Application.Endpoints.UserEndpoints.CreateUser;

internal record CreateUserRequest
(
    string FullName,
    string Email,
    string UserName,
    string Password
);