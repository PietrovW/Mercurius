using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.Data;

public sealed class MercuriusContext:DbContext
{
    public MercuriusContext(DbContextOptions options) : base(options) { }
    public DbSet<ExceptionInfoEntitie> ExceptionInfos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static async Task InitializeAsync(MercuriusContext db)
    {
        await db.Database.MigrateAsync();
    }
   
}
