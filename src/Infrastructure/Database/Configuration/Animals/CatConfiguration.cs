using Domain.Animals;
using Domain.ValueObjects;
using Infrastructure.Database.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.Configuration.Animals;

internal sealed class CatConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        AnimalConfiguration.Configure(builder);
        builder.Property(c => c.Colour)
            .IsRequired()
            .HasConversion(new DescriptionConverter())
            .HasMaxLength(Description.MaxLength);
    }
}

