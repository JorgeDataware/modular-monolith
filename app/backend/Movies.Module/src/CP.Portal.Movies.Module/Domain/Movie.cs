using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Portal.Movies.Module.Domain;

internal class Movie
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateOnly ReleaseYear { get; private set; }
    public int DurationMinutes { get; private set; }
    public string Language { get; private set; }
    public decimal RentalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    //Relaciones
    public ICollection<Cast> Casts { get; private set; } = [];
    public ICollection<Crew> Crewers { get; private set; } = [];
    public ICollection<MovieGenre> MovieGenres { get; private set; } = [];

    // Proyecciones
    [NotMapped]
    public IEnumerable<Genre> Genres => MovieGenres.Select(mg => mg.Genre!).Where(mg => mg != null);
    [NotMapped]
    public IEnumerable<Person> Actors => Casts.Select(c => c.Person!).Where(c => c != null);
    [NotMapped]
    public IEnumerable<Person> Staff => Crewers.Select(c => c.Person!).Where(c => c != null);
}
