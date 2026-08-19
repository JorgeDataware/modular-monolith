using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.ListPersons;

internal class ListPersonsEndpoint(IPersonService personService) : EndpointWithoutRequest<IEnumerable<ListPersonDto>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Get("api/Persons");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _personService.ListPersonsAsync(ct);

        await this.SendApiResponseAsync(result, "Personas obtenidas correctamente", ct: ct);
    }
}
