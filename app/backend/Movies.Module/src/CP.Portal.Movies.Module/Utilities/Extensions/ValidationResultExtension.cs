using CP.Portal.Movies.Module.Utilities.Abstractions;
using FluentValidation.Results;

namespace CP.Portal.Movies.Module.Utilities.Extensions;

internal static class ValidationResultExtension
{
    public static Result<T> ToFailure<T>(this ValidationResult validation)
    {
        var message = string.Join(";", validation.Errors.
            Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

        return Result<T>.Failure(Error.Validation(message));
    }
}
