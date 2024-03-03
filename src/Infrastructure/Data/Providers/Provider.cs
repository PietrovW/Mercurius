namespace Infrastructure.Data.Providers;

public record Provider(string Name, string Assembly)
{
    public static readonly Provider MSSql = new(nameof(MSSql), typeof(MSSql.Marker).Assembly.GetName().Name!);
    public static readonly Provider MySql = new(nameof(MySql), typeof(MySql.Marker).Assembly.GetName().Name!);
    public static readonly Provider Postgres = new(nameof(Postgres), typeof(Postgres.Marker).Assembly.GetName().Name!);
}
