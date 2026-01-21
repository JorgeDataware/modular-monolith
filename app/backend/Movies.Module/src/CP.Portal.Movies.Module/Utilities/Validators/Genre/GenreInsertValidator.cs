using CP.Portal.Movies.Module.Application.Endpoints.GenreEndpoints.CreateGenre;
using FluentValidation;

namespace CP.Portal.Movies.Module.Utilities.Validators.Genre;

internal class GenreInsertValidator : AbstractValidator<AddGenreRequest>
{
    public GenreInsertValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del género es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre del género no puede exceder los 50 caracteres.");
    }
}
