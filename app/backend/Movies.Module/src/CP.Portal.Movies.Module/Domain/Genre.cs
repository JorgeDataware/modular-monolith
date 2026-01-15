namespace CP.Portal.Movies.Module.Domain;

internal class Genre
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Name { get; private set; }

    //Relaciones
    public ICollection<MovieGenre> MovieGenres { get; private set; } = [];
}
