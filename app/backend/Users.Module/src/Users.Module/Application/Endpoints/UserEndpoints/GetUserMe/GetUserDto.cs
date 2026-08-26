namespace Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;

internal record GetUserDto
(
    string Id,
    string FullName,
    string Email,
    string UserName
);
