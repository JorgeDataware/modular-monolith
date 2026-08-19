using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CP.Portal.Movies.Module.Domain.Repositories.PersonRepository;

internal class PersonRepository(MovieDbContext context) : IPersonRepository
{
    private readonly MovieDbContext _context = context;

    public async Task AddPersonAsync(Person person, CancellationToken ct)
        => await _context.persons.AddAsync(person, ct);

    public async Task<int> DeletePerson(Guid Id, CancellationToken ct)
        => await _context.persons.Where(p => p.Id == Id).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Person>> GetAllPersonsAsync(CancellationToken ct)
        => await _context.persons.AsNoTracking().ToListAsync(ct);

    public async Task<Person?> GetPersonByIdAsync(Guid Id, CancellationToken ct)
        => await _context.persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);

    public async Task<Person?> GetPersonTrackedByIdAsync(Guid Id, CancellationToken ct)
        => await _context.persons.FirstOrDefaultAsync(p => p.Id == Id);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}
