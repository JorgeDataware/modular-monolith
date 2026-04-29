namespace CP.Portal.Movies.Module.Utilities.Abstractions;

/// <summary>
/// DTO de respuesta estandarizada para APIs REST.
/// Proporciona una estructura consistente para todas las respuestas HTTP.
/// </summary>
/// <typeparam name="T">Tipo de dato que se incluirá en la respuesta</typeparam>
public class ResponseDto<T>
{
    /// <summary>
    /// Código o descripción del error (null si la operación fue exitosa)
    /// </summary>
    public string? Error { get; set; }
    
    /// <summary>
    /// Mensaje descriptivo de la operación
    /// </summary>
    public string Message { get; set; } = null!;
    
    /// <summary>
    /// Lista de datos de respuesta (null si hubo error)
    /// </summary>
    public List<T>? Data { get; set; }
}
