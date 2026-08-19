namespace CP.Portal.Movies.Module.Utilities.Abstractions;

public class Error
{
    private string v;

    public string Code { get; }
    public string Message { get; }
    public int HttpCode { get; }

    public Error(string code, string message, int httpCode = 400)
    {
        Code = code;
        Message = message;
        HttpCode = httpCode;
    }

    public Error(string v)
    {
        this.v = v;
    }
}
