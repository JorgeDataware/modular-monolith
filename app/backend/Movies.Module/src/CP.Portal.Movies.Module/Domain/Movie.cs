using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Portal.Movies.Module.Domain;

internal class Movie
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public DateOnly ReleaseYear { get; set; }
    public int DurationMinutes { get; set; }
    public string Language { get; set; } = null!;
    public decimal RentalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //Relaciones
    public ICollection<Cast> Casts { get; set; } = [];
    public ICollection<Crew> Crewers { get; set; } = [];
    public ICollection<MovieGenre> MovieGenres { get; set; } = [];

    // Proyecciones
    [NotMapped]
    public IEnumerable<Genre> Genres => MovieGenres.Select(mg => mg.Genre!).Where(mg => mg != null);
    [NotMapped]
    public IEnumerable<Person> Actors => Casts.Select(c => c.Person!).Where(c => c != null);
    [NotMapped]
    public IEnumerable<Person> Staff => Crewers.Select(c => c.Person!).Where(c => c != null);
}
