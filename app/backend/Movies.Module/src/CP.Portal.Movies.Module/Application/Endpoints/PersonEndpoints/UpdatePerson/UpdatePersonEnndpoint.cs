using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.UpdatePerson;

internal class UpdatePersonEnndpoint(IPersonService personService) : Endpoint<UpdatePersonRequest, ApiResponse<Guid>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Patch("/api/Person/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePersonRequest request, CancellationToken ct)
    {
        var result = await _personService.UpdatePersonAsync(request, ct);

        await this.SendApiResponseAsync(result, "Persona actualizada correctamente");
    }
}
