namespace CP.Portal.Movies.Module.Domain;

internal class MovieGenre
{
    public Guid MovieId { get; private set; }
    public Guid GenreId { get; private set; }

    public Movie Movie { get; private set; } = null!;
    public Genre Genre { get; private set; } = null!;
}
