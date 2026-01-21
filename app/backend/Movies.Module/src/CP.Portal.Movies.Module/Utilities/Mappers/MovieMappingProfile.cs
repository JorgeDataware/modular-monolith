using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using CP.Portal.Movies.Module.Domain;

namespace CP.Portal.Movies.Module.Utilities.Mappers;

internal class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, AddMovieRequest>().ReverseMap();
    }
}
