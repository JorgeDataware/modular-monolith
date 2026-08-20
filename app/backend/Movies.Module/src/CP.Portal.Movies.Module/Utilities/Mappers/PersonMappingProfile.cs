using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.UpdatePerson;
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
        CreateMap<UpdatePersonRequest, Person>()
            // La PK y los campos fuera del alcance del PATCH no se tocan.
            .ForMember(p => p.Id, o => o.Ignore())
            .ForMember(p => p.BirthDate, o => o.Ignore())
            .ForMember(p => p.Casts, o => o.Ignore())
            .ForMember(p => p.Crewers, o => o.Ignore());
            // Semántica PATCH: lo que no viene en el request conserva su valor actual.
            //.ForMember(p => p.FirstName, o => o.Condition((_, _, value) => value is not null))
            //.ForMember(p => p.LastName, o => o.Condition((_, _, value) => value is not null))
            //.ForMember(p => p.Bio, o => o.Condition((_, _, value) => value is not null));
    }
}
