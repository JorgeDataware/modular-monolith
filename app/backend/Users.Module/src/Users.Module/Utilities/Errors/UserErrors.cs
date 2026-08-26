using Core.Contracts.Abstractions;

namespace Users.Module.Utilities.Errors;

internal class UserErrors
{
    internal static Error UserNotFound => new Error("UserNotFound", "El usuario no fue encontrado", 404);
    internal static Error IvalidCredentials => new Error("IvalidCredentials", "Las credenciales ingresadas son incorrectas", 401);
    internal static Error CurrentUserNotFound => new Error("CurrentUserNotFound", "El usuario logueado no existe en el sistema", 401);
}
