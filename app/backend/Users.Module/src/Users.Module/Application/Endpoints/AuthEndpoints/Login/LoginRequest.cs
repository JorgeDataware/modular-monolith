namespace Users.Module.Application.Endpoints.AuthEndpoints.Login;

internal record LoginRequest
(
    string Email,
    string Password
);
