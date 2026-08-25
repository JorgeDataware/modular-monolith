namespace Users.Module.Utilities.Configuration;

internal sealed class JWTConfigs
{
    internal const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; }
}
