using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Users.Module.Utilities.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, [NotNullWhen(true)] out string? userId)
    {
        userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? user.FindFirst("sub")?.Value;

        return !string.IsNullOrWhiteSpace(userId);
    }
}
