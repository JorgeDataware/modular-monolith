using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;

internal class GetPersonByIdEndpoint(IPersonService personService) : EndpointWithoutRequest<ApiResponse<GetPersonDto>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Get("api/Persons/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");
        var result = await _personService.GetPersonByIdAsync(id, ct);

        await this.SendApiResponseAsync(result, "Persona obtenida correctamente", ct: ct);
    }
}
