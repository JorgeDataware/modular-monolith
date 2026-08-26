using FluentValidation;
using Users.Module.Application.Endpoints.UserEndpoints.CreateUser;

namespace Users.Module.Utilities.Validators;

internal class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        // Aplica el comportamiento de parada a todas las cadenas de reglas de esta clase
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("El email debe de tener un formato válido");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(50).WithMessage("El nombre no debe de ser más largo de 50 caracteres");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El usuario es obligatorio.")
            .Matches(@"^\S+$").WithMessage("El texto no puede contener espacios en blanco.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatorio.")
            .MinimumLength(8).WithMessage("La contraseña debe de tener mínimo 8 caracteres")
            .MaximumLength(16).WithMessage("La contraseña debe de tener máximo 16 caracteres");
    }
}
