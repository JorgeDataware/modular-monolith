using Core.Contracts.Abstractions;
using FluentValidation.Results;

namespace Core.Contracts.Extensions;

public static class ValidationResultExtension
{
    public static Result<T> ToFailure<T>(this ValidationResult validation)
    {
        var message = string.Join(";", validation.Errors.
            Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

        return Result<T>.Failure(Error.Validation(message));
    }
}
