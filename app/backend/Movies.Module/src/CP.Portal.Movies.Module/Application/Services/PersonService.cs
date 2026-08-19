using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Application.Services;

internal class PersonService : IPersonService
{
    public Task<Result<Guid>> AddPersonAsync(AddPersonRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> DeletePersonAsync(Guid Id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
