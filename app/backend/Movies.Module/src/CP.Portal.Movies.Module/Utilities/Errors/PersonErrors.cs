using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Utilities.Errors;

internal class PersonErrors
{
    internal static Error PersonNotFound => new("PersonNotFound", "Este usuario no existe", 404);
}
