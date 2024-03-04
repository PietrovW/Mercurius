namespace Api.Providers;

public record Provider(string Name, string Assembly)
{
    public static readonly Provider SqlServer = new(nameof(SqlServer), typeof(Infrastructure.SqlServer.Marker).Assembly.GetName().Name!);
    public static readonly Provider MySql = new(nameof(MySql), typeof(Infrastructure.MySql.Marker).Assembly.GetName().Name!);
    public static readonly Provider Postgres = new(nameof(Postgres), typeof(Infrastructure.Postgres.Marker).Assembly.GetName().Name!);
}
