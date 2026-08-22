using Core.Contracts.Abstractions;

namespace Users.Module.Utilities.Errors;

internal class UserErrors
{
    internal static Error UserNotFound => new Error("UserNotFound", "El usuario no fue encontrado", 404);
}
