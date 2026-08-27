using Core.Contracts.Abstractions;

namespace Movies.Module.Contracts.Errors;

/// <summary>
/// Errores que el módulo de películas puede devolver a otros módulos.
/// Son públicos a propósito: forman parte del contrato, igual que los mensajes.
/// Los errores internos del módulo siguen viviendo en Utilities/Errors y no se exponen.
/// </summary>
public static class MoviesContractErrors
{
    public static Error MovieNotFound => new("MovieNotFound", "La película solicitada no existe", 404);
}
