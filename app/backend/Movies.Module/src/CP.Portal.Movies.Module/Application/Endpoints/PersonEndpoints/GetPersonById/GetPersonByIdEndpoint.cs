using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.GetPersonById;

internal class GetPersonByIdEndpoint(IPersonService personService) : EndpointWithoutRequest<ApiResponse<GetPersonDto>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Get("/api/Person/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");
        var result = await _personService.GetPersonByIdAsync(id, ct);

        await this.SendApiResponseAsync(result, "Persona obtenida correctamente", ct: ct);
    }
}
