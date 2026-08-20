using CP.Portal.Movies.Module.Application.Endpoints.PersonEndpoints.UpdatePerson;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CP.Portal.Movies.Module.Utilities.Validators.Person;

internal class PersonUpdateValidator : AbstractValidator<UpdatePersonRequest>
{
    public PersonUpdateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre de la persona es obligatorio")
            .MaximumLength(50).WithMessage("El nombre de la persona no puede exceder los 50 caracteres");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido de la persona es obligatorio")
            .MaximumLength(50).WithMessage("El apellido de la persona no puede exceder los 50 caracteres");
        RuleFor(x => x.Bio)
            .NotEmpty().WithMessage("La bio de la persona es obligatoria")
            .MaximumLength(250).WithMessage("La bio de la persona no puede exceder los 250 caracteres");
    }
}
