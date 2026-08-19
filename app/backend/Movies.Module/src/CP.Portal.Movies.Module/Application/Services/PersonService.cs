using AutoMapper;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.AddPersonAsync;
using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.ListPersons;
using CP.Portal.Movies.Module.Application.Services.IServices;
using CP.Portal.Movies.Module.Domain;
using CP.Portal.Movies.Module.Domain.Repositories.PersonRepository;
using CP.Portal.Movies.Module.Utilities.Abstractions;
using CP.Portal.Movies.Module.Utilities.Errors;
using CP.Portal.Movies.Module.Utilities.Extensions;
using FluentValidation;

namespace CP.Portal.Movies.Module.Application.Services;

internal class PersonService(IPersonRepository personRepository, IValidator<AddPersonRequest> validator, IMapper mapper) : IPersonService
{
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly IValidator<AddPersonRequest> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<string>> AddPersonAsync(AddPersonRequest request, CancellationToken ct)
    {
        var val = await _validator.ValidateAsync(request);

        if (!val.IsValid)
            return val.ToFailure<string>();

        var person = _mapper.Map<Person>(request);

        await _personRepository.AddPersonAsync(person, ct);
        await _personRepository.SaveChangesAsync(ct);

        return Result<string>.Success(person.Id.ToString());
    }

    public async Task<Result<Guid>> DeletePersonAsync(Guid Id, CancellationToken ct)
    {
        var result = await _personRepository.DeletePerson(Id, ct);

        if (result < 1)
            return Result<Guid>.Failure(PersonErrors.PersonNotFound);

        return Result<Guid>.Success(Id);
    }

    public async Task<Result<IEnumerable<ListPersonDto>>> ListPersonsAsync(CancellationToken ct)
    {
        var result = await _personRepository.GetAllPersonsAsync(ct);

        return Result<IEnumerable<ListPersonDto>>.Success(result.Select(p => new ListPersonDto(
                p.Id,
                p.FirstName,
                p.LastName
            )));
    }
}
