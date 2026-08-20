using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.DeletePerson;

internal class DeletePersonEndpoint(IPersonService personService) : EndpointWithoutRequest<ApiResponse<Guid>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Delete("/api/Person/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");
        var result = await _personService.DeletePersonAsync(id, ct);

        await this.SendApiResponseAsync(result, "Persona eliminada exitosamente", ct: ct);
    }
}
