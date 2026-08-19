using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Domain.Repositories.PersonRepository;

internal interface IPersonRepository
{
    Task AddPersonAsync(Person person, CancellationToken ct);
    Task DeletePerson(Guid Id, CancellationToken ct);
}
