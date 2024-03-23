namespace Api.Options;

public record KeycloakOptions
{
    public const string Keycloak = nameof(Keycloak);
    public string Realm { get; init; }
    public string AuthServerUrl { get; init; }
    public bool SSLRequired { get; init; } = false;
    public string Resource { get; init; }
    public string Secret { get; init; }
    public bool VerifyTokenAudience { get; init; } = true;
}
