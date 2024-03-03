using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

internal class ExceptionInfoConfiguration : IEntityTypeConfiguration<ExceptionInfoEntitie>
{
    public void Configure(EntityTypeBuilder<ExceptionInfoEntitie> builder)
    {
        builder.HasKey(ci => ci.Id);
    }
}
