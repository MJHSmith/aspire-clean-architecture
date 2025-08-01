using Domain.Animals;
using Domain.ValueObjects;
using Infrastructure.Database.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Animals;

internal static class AnimalConfiguration
{
    internal static void Configure<T>(EntityTypeBuilder<T> builder) 
        where T : AnimalEntity
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name)
            .IsRequired()
            .HasConversion(new NameConverter())
            .HasMaxLength(Name.MaxLength);
    }
}



