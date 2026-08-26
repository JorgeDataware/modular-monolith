using AutoMapper;
using Core.Contracts.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Users.Module.Application.Endpoints.UserEndpoints.CreateUser;
using Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;
using Users.Module.Domain;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.UserService;

internal class UserService(UserManager<User> userManager, IMapper mapper, IValidator<CreateUserRequest> createUserValidator) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateUserRequest> _createUserValidator = createUserValidator;

    public async Task<Result<string>> CreateUserAsync(CreateUserRequest request, CancellationToken ct)
    {
        var val = await _createUserValidator.ValidateAsync(request, ct);

        if (!val.IsValid)
            return Result<string>.Failure(new Error("ValidationError", (val.Errors.FirstOrDefault()!).ErrorMessage));

        var newUser = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            FullName = request.FullName
        };

        await _userManager.CreateAsync(newUser, request.Password).WaitAsync(ct);

        return Result<string>.Success(newUser.Id);
    }

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
