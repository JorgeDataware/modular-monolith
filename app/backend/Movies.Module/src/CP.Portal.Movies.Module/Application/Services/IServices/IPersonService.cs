using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.ListPersons;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IPersonService
{
    Task<Result<string>> AddPersonAsync(AddPersonRequest request, CancellationToken ct);
    Task<Result<Guid>> DeletePersonAsync(Guid Id, CancellationToken ct);
    Task<Result<IEnumerable<ListPersonDto>>> ListPersonsAsync(CancellationToken ct);
}
