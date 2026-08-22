namespace Core.Contracts.Abstractions;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public int HttpCode { get; }

    public Error(string code, string message, int httpCode = 400)
    {
        Code = code;
        Message = message;
        HttpCode = httpCode;
    }

    public static Error Validation(string message) => new("ValidationError", message, 400);

}
