using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;
using CP.Portal.Movies.Module.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Utilities.Mappers;

internal class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<Person, AddPersonRequest>().ReverseMap();
        CreateMap<Person, GetPersonDto>().ReverseMap();
    }
}
