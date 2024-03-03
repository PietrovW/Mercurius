using Application.Entities;
using Microsoft.EntityFrameworkCore;
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
}
