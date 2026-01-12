using CP.Portal.Movies.Module.Application.Services.IServices;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.GetListMovies;

internal class GetListMoviesEndpoint(GetListMoviesService movieService) : EndpointWithoutRequest<IEnumerable<MovieDto>>
{
    private readonly GetListMoviesService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/movies/GetMovies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct=default)
    {
        var movies = await _movieService.GetMovies();
        
        //var response = new ListMoviesDto
        //{
        //    Movies = movies.ToList()
        //};

        await Send.OkAsync(movies.ToList());
    }
}
