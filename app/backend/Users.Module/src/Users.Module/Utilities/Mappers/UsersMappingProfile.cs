using AutoMapper;
using Users.Module.Application.Endpoints.UserEndpoints.GetUserMe;
using Users.Module.Domain;

namespace Users.Module.Utilities.Mappers;

internal class UsersMappingProfile : Profile
{
    public UsersMappingProfile()
    {
        CreateMap<User, GetUserDto>().ReverseMap();
    }
}
