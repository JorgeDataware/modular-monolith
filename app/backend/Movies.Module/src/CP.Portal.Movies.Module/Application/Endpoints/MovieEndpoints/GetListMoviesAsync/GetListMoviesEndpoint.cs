using CP.Portal.Movies.Module.Application.Services.IServices;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.GetListMoviesAsync;

internal class GetListMoviesEndpoint(IMovieService movieService) : EndpointWithoutRequest<IEnumerable<MovieDto>>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/movies/GetMovies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct=default)
    {
        var movies = await _movieService.ListMovieAsync(ct);

        await Send.OkAsync(movies.Value.Movies, ct);
    }
}
