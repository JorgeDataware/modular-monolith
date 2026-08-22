using Core.Contracts.Abstractions;
using FastEndpoints;

namespace Core.Contracts.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Mapea un Result&lt;T&gt; del service/repository al envelope de respuesta HTTP.
    /// </summary>
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, string? successMessage = null)
    {
        return result.IsSuccess
            ? ApiResponse<T>.Ok(result.Value, successMessage)
            // El valor viaja también en el fallo: los errores accionables lo llenan con el
            // detalle del conflicto, y los demás lo dejan en su default (null).
            : ApiResponse<T>.Fail(result.Error.Code, result.Error.Message, result.Value);
    }

    /// <summary>
    /// Mapea el Result&lt;T&gt; al envelope y lo escribe en la respuesta HTTP.
    /// El código de estado sale de Error.HttpCode cuando el Result es fallido.
    /// </summary>
    public static Task SendApiResponseAsync<T>(
        this IEndpoint endpoint,
        Result<T> result,
        string? successMessage = null,
        int successStatusCode = 200,
        CancellationToken ct = default)
        => endpoint.HttpContext.Response.SendAsync(
            result.ToApiResponse(successMessage),
            result.IsSuccess ? successStatusCode : result.Error.HttpCode,
            cancellation: ct);
}
