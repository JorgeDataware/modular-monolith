using AutoMapper;
using Core.Contracts.Abstractions;
using Microsoft.AspNetCore.Identity;
using Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;
using Users.Module.Domain;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.UserService;

internal class UserService(UserManager<User> userManager, IMapper mapper) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<GetUserDto>> GetUserByIdAsync(string? Id, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Id))
            return Result<GetUserDto>.Failure(UserErrors.IvalidCredentials);

        var user = await _userManager.FindByIdAsync(Id).WaitAsync(ct);

        if (user is null)
            return Result<GetUserDto>.Failure(UserErrors.CurrentUserNotFound);

        var userResponse = _mapper.Map<GetUserDto>(user);

        return Result<GetUserDto>.Success(userResponse);
    }
}
