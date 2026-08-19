using CP.Portal.Movies.Module.Utilities.Abstractions;

namespace CP.Portal.Movies.Module.Utilities.Errors;

internal class MovieErrors
{
    internal static Error MovieNotFound => new("MovieNotFound", "The Movie was not found", 404);
}
