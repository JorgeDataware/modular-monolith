using CP.Portal.Movies.Module.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Domain.Repositories.PersonRepository;

internal class PersonRepository (MovieDbContext context) : IPersonRepository
{
    private readonly MovieDbContext _context = context;

    public async Task AddPersonAsync(Person person, CancellationToken ct)
        => await _context.persons.AddAsync(person, ct);

    public async Task DeletePerson(Guid Id, CancellationToken ct)
        => await _context.persons.Where(p => p.Id == Id).ExecuteDeleteAsync(ct);
}
