using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.Data;

public sealed class MercuriusContext:DbContext
{
    public MercuriusContext(DbContextOptions<MercuriusContext> options) : base(options) { }
    public DbSet<ExceptionInfoEntitie> ExceptionInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static async Task InitializeAsync(MercuriusContext db)
    {
        await db.Database.MigrateAsync();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfiguration Configuration = builder.Build();
        
        optionsBuilder.UseNpgsql(
            Configuration.GetConnectionString("DefaultConnection"));
        base.OnConfiguring(optionsBuilder);
    }
}
