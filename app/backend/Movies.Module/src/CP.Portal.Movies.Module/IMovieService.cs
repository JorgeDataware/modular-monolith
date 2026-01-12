namespace CP.Portal.Movies.Module;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetMovies();
}
