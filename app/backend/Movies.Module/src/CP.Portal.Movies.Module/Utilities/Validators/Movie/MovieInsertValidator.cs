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
    }
}
