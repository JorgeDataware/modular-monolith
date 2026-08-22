namespace Core.Contracts.Abstractions;

public class ApiResponse<T>
{
    public string? Error { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Data = data, Message = message };

    public static ApiResponse<T> Fail(string error, string message, T? data = default) =>
        new() { Error = error, Message = message, Data = data };
}
