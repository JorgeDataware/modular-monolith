namespace CP.Portal.Movies.Module.Utilities.Abstractions;

/// <summary>
/// Helpers opcionales para crear respuestas HTTP estandarizadas.
/// NOTA: Se recomienda usar los extension methods de EndpointExtensions en su lugar.
/// </summary>
internal interface IHttpHelpers
{
    ResponseDto<T> CreateFailureResponse<T>(string error, string errorMessage);
    ResponseDto<T> CreateSuccessResponse<T>(T data, string message);
}

/// <summary>
/// Implementación de helpers para respuestas HTTP.
/// NOTA: Se recomienda usar los extension methods de EndpointExtensions en su lugar.
/// </summary>
internal class HttpHelpers : IHttpHelpers
{
    public ResponseDto<T> CreateFailureResponse<T>(string error, string errorMessage)
    {
        return new ResponseDto<T>
        {
            Error = error,
            Message = errorMessage,
            Data = null
        };
    }

    public ResponseDto<T> CreateSuccessResponse<T>(T data, string message)
    {
        return new ResponseDto<T>
        {
            Data = new List<T> { data },
            Message = message,
            Error = null
        };
    }
}
