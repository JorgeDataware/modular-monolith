using CP.Portal.Movies.Module.Utilities.Abstractions;
using FastEndpoints;

namespace CP.Portal.Movies.Module.Utilities.Extensions;

/// <summary>
/// Extension methods para endpoints de FastEndpoints que proporcionan respuestas HTTP estandarizadas.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Envía una respuesta de éxito estandarizada con una colección de elementos.
    /// NOTA: Esta sobrecarga tiene prioridad para evitar ambigüedad con List{T}.
    /// </summary>
    /// <typeparam name="T">Tipo de dato de los elementos</typeparam>
    /// <param name="endpoint">El endpoint actual</param>
    /// <param name="data">La colección de datos a incluir en la respuesta</param>
    /// <param name="message">Mensaje descriptivo del éxito</param>
    /// <param name="statusCode">Código de estado HTTP (por defecto 200)</param>
    /// <param name="ct">Token de cancelación</param>
    public static Task SendStandardSuccessAsync<T>(
        this IEndpoint endpoint,
        IEnumerable<T> data,
        string message = "Operación exitosa",
        int statusCode = 200,
        CancellationToken ct = default)
    {
        var response = new ResponseDto<T>
        {
            Data = data?.ToList() ?? new List<T>(),
            Message = message,
            Error = null
        };
        return endpoint.HttpContext.Response.SendAsync(response, statusCode, cancellation: ct);
    }

    /// <summary>
    /// Envía una respuesta de éxito estandarizada con un único elemento de datos.
    /// </summary>
    /// <typeparam name="T">Tipo de dato a enviar</typeparam>
    /// <param name="endpoint">El endpoint actual</param>
    /// <param name="data">Los datos a incluir en la respuesta</param>
    /// <param name="message">Mensaje descriptivo del éxito</param>
    /// <param name="statusCode">Código de estado HTTP (por defecto 200)</param>
    /// <param name="ct">Token de cancelación</param>
    public static Task SendStandardSuccessAsync<T>(
        this IEndpoint endpoint,
        T data,
        string message = "Operación exitosa",
        int statusCode = 200,
        CancellationToken ct = default) where T : notnull
    {
        var response = new ResponseDto<T>
        {
            Data = new List<T> { data },
            Message = message,
            Error = null
        };
        return endpoint.HttpContext.Response.SendAsync(response, statusCode, cancellation: ct);
    }

    /// <summary>
    /// Envía una respuesta de error estandarizada.
    /// </summary>
    /// <typeparam name="T">Tipo de dato (generalmente object para errores)</typeparam>
    /// <param name="endpoint">El endpoint actual</param>
    /// <param name="error">Código o tipo de error</param>
    /// <param name="message">Mensaje descriptivo del error</param>
    /// <param name="statusCode">Código de estado HTTP (por defecto 400)</param>
    /// <param name="ct">Token de cancelación</param>
    public static Task SendStandardFailureAsync<T>(
        this IEndpoint endpoint,
        string error,
        string message,
        int statusCode = 400,
        CancellationToken ct = default)
    {
        var response = new ResponseDto<T>
        {
            Data = null,
            Message = message,
            Error = error
        };
        return endpoint.HttpContext.Response.SendAsync(response, statusCode, cancellation: ct);
    }

    /// <summary>
    /// Envía una respuesta estandarizada basada en un objeto Result{T}.
    /// Maneja automáticamente éxito y fallo según el estado del Result.
    /// </summary>
    /// <typeparam name="T">Tipo de dato del Result</typeparam>
    /// <param name="endpoint">El endpoint actual</param>
    /// <param name="result">El objeto Result a procesar</param>
    /// <param name="successMessage">Mensaje a usar en caso de éxito</param>
    /// <param name="successStatusCode">Código de estado HTTP para éxito (por defecto 200)</param>
    /// <param name="errorStatusCode">Código de estado HTTP para error (por defecto 400)</param>
    /// <param name="ct">Token de cancelación</param>
    public static Task SendStandardResponseAsync<T>(
        this IEndpoint endpoint,
        Result<T> result,
        string successMessage = "Operación exitosa",
        int successStatusCode = 200,
        int errorStatusCode = 400,
        CancellationToken ct = default)
    {
        if (result.IsSuccess)
        {
            var response = new ResponseDto<T>
            {
                Data = new List<T> { result.Value },
                Message = successMessage,
                Error = null
            };
            return endpoint.HttpContext.Response.SendAsync(response, successStatusCode, cancellation: ct);
        }
        else
        {
            var response = new ResponseDto<T>
            {
                Data = null,
                Message = result.Error?.Message ?? "Error en la operación",
                Error = result.Error?.Code ?? "UnknownError"
            };
            return endpoint.HttpContext.Response.SendAsync(response, errorStatusCode, cancellation: ct);
        }
    }
}
