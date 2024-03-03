namespace Api.Options;

public record MercuriusOptions
{
    public const string Mercurius = nameof(Mercurius);
    public string Provider { get; init; } =string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
}
