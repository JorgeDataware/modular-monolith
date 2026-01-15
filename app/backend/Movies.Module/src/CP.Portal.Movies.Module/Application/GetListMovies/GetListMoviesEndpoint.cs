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

        await Send.OkAsync(movies.ToList());
    }
}
