using FastEndpoints;

namespace CP.Portal.Movies.Module;

public class GetListMoviesEndpoint(IMovieService movieService) : EndpointWithoutRequest<ListMoviesDto>
{
    private readonly IMovieService _movieService = movieService;

    public override void Configure()
    {
        Get("/api/movies/GetMovies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct=default)
    {
        var movies = await _movieService.GetMovies();
        
        var response = new ListMoviesDto
        {
            Movies = movies.ToList()
        };

        await Send.OkAsync(movies.ToList());
    }
}
