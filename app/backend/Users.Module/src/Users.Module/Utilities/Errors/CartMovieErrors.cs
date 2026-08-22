using Core.Contracts.Abstractions;

namespace Users.Module.Utilities.Errors;

internal class CartMovieErrors
{
    internal static Error MovieIdEmpy => new Error("MovieIdEmpy", "Se necesita una película para agregarla al carrito", 400);
    internal static Error MovieNotFound => new Error("MovieNotFound", "No se encontró la película", 404);
}
