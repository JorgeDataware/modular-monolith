namespace CP.Portal.Movies.Module.Utilities.Abstractions;

public class Result<T>
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public T Value { get; }

    protected Result(bool isSuccess, Error error, T value)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, null, value);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(false, error, default);
    }
}
