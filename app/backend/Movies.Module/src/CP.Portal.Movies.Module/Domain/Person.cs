namespace CP.Portal.Movies.Module.Domain;

internal class Person
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Bio { get; private set; }

    //Relaciones
    public ICollection<Cast> Casts { get; private set; } = [];
    public ICollection<Crew> Crewers { get; private set; } = [];
}
