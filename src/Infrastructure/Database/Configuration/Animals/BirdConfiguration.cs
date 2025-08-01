using Domain.Animals;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Animals;

internal sealed class BirdConfiguration : IEntityTypeConfiguration<Bird>
{
    public void Configure(EntityTypeBuilder<Bird> builder)
    {
        AnimalConfiguration.Configure(builder);
        builder.Property(c => c.WingspanInCentimeters)
            .HasColumnName("Wingspan")
            .IsRequired();
    }
}

