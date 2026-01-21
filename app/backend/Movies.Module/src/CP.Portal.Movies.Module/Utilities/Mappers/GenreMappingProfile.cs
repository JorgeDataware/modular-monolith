using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.CreateGenre;
using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.GetListGenresAsync;
using CP.Portal.Movies.Module.Domain;

namespace CP.Portal.Movies.Module.Utilities.Mappers;

internal class GenreMappingProfile : Profile
{
    public GenreMappingProfile()
    {
        CreateMap<Genre, AddGenreRequest>().ReverseMap();
        CreateMap<IEnumerable<Genre>, IEnumerable<GenreDto>>().ReverseMap();
    }
}
