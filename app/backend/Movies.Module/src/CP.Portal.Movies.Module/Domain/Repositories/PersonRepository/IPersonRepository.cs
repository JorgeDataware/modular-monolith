namespace CP.Portal.Movies.Module.Domain.Repositories.PersonRepository;

internal interface IPersonRepository
{
    Task AddPersonAsync(Person person, CancellationToken ct);
    Task<int> DeletePerson(Guid Id, CancellationToken ct);
    Task<IEnumerable<Person>> GetAllPersonsAsync(CancellationToken ct);
    Task<Person?> GetPersonByIdAsync(Guid Id, CancellationToken ct);
    Task<Person?> GetPersonTrackedByIdAsync(Guid Id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
