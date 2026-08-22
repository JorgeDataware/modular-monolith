using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.ListPersons;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.UpdatePerson;
using Core.Contracts.Abstractions;

namespace CP.Portal.Movies.Module.Application.Services.IServices;

internal interface IPersonService
{
    Task<Result<string>> AddPersonAsync(AddPersonRequest request, CancellationToken ct);
    Task<Result<Guid>> DeletePersonAsync(Guid Id, CancellationToken ct);
    Task<Result<IEnumerable<ListPersonDto>>> ListPersonsAsync(CancellationToken ct);
    Task<Result<GetPersonDto>> GetPersonByIdAsync(Guid Id, CancellationToken ct);
    Task<Result<Guid>> UpdatePersonAsync(UpdatePersonRequest request, CancellationToken ct);
}
