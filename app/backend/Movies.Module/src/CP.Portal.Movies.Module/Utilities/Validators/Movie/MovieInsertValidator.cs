using CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;
using FluentValidation;

namespace CP.Portal.Movies.Module.Utilities.Validators.Movie;

internal class MovieInsertValidator : AbstractValidator<AddMovieRequest>
{
    public MovieInsertValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(1000).WithMessage("La descripción no puede exceder los 1000 caracteres.");
        RuleFor(x => x.ReleaseYear)
            .NotEmpty().WithMessage("La fecha de lanzamiento es obligatoria.")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("La fecha de lanzamiento no puede ser en el futuro.");
        RuleFor(x => x.RentalPrice)
            .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");
        RuleFor(x => x.Genres)
            .NotEmpty().WithMessage("Debe seleccionar al menos un género.");

        RuleFor(x => x.Casters)
            .NotEmpty().WithMessage("El elenco no puede estar vacío.")
            .Must(NoRepeatedPersons).WithMessage("El elenco no puede repetir la misma persona.");
        // Valida cada elemento de la colección: PersonId y rol obligatorios.
        RuleForEach(x => x.Casters)
            .ChildRules(participant => ParticipantRules(participant, "el elenco"));

        RuleFor(x => x.Crewers)
            .NotEmpty().WithMessage("El staff no puede estar vacío.")
            .Must(NoRepeatedPersons).WithMessage("El staff no puede repetir la misma persona.");
        RuleForEach(x => x.Crewers)
            .ChildRules(participant => ParticipantRules(participant, "el staff"));
    }

    /// <param name="participantKind">Nombre de la colección, para que el mensaje diga si falla el elenco o el staff.</param>
    private static void ParticipantRules(InlineValidator<Participant> validator, string participantKind)
    {
        validator.RuleFor(p => p.PersonId)
            .NotEmpty().WithMessage($"La persona en {participantKind} es obligatoria.");

        validator.RuleFor(p => p.role)
            .NotEmpty().WithMessage($"El rol en {participantKind} es obligatorio.");
    }

    // Cast y Crew usan (MovieId, PersonId) como clave primaria: una persona repetida
    // dentro de la misma colección rompería al guardar.
    private static bool NoRepeatedPersons(IEnumerable<Participant> participants)
        => participants is null || participants.DistinctBy(p => p.PersonId).Count() == participants.Count();
}
