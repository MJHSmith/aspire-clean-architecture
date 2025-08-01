using Domain.Animals;
using Domain.ValueObjects;
using Infrastructure.Database.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Animals;

internal sealed class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        AnimalConfiguration.Configure(builder);
        builder.Property(d => d.Colour1)
            .IsRequired()
            .HasConversion(new ColourConverter())
            .HasMaxLength(Colour.MaxLength);

        builder.Property(d => d.Colour2)
            .HasConversion(new NullableColourConverter())
            .HasMaxLength(Colour.MaxLength);

        builder.Property(d =>d.Pattern)
            .HasConversion(new NullableDescriptionConverter())
            .HasMaxLength(Description.MaxLength);
    }
}

