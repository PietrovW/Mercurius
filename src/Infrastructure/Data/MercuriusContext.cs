using Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;

public sealed class MercuriusContext:DbContext
{
    public MercuriusContext(DbContextOptions<MercuriusContext> options) : base(options) { }
    public DbSet<ExceptionInfoEntitie> ExceptionInfos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static async Task InitializeAsync(MercuriusContext db)
    {
        if (db.Database!=null && (!db.Database.ProviderName.Contains("InMemory")))
        {
            await db.Database.MigrateAsync();
            if (db.ExceptionInfos.Any())
                return;
        }
    }
}
