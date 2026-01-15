namespace CP.Portal.Movies.Module.Domain;

internal class Crew
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid MovieId { get; private set; }
    public Guid PersonId { get; private set; }
    public string Role { get; private set; }

    public Movie Movie { get; private set; } = null!;
    public Person Person { get; private set; } = null!;
}
