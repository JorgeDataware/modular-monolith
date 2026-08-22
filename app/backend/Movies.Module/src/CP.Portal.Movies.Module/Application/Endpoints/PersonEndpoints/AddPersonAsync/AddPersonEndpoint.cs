using CP.Portal.Movies.Module.Application.Services.IServices;
using Core.Contracts.Abstractions;
using Core.Contracts.Extensions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;

internal class AddPersonEndpoint(IPersonService personService) : Endpoint<AddPersonRequest, ApiResponse<Guid>>
{
    private readonly IPersonService _personService = personService;

    public override void Configure()
    {
        Post("/api/Person");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddPersonRequest request, CancellationToken ct)
    {
        var result = await _personService.AddPersonAsync(request, ct);

        await this.SendApiResponseAsync(result, "Persona agregada correctamente", 201, ct: ct);
    }
}
