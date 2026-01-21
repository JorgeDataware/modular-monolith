namespace CP.Portal.Movies.Module.Domain;

internal class Genre
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; }

    //Relaciones
    public ICollection<MovieGenre> MovieGenres { get; private set; } = [];
}
