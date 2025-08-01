using Domain.Animals;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Animals;

internal sealed class BirdSeedData : IEntityTypeConfiguration<Bird>
{
    public void Configure(EntityTypeBuilder<Bird> builder)
    {
        builder.HasData(
            new
            {
                Id = Guid.Parse("a1e1e1e1-1111-1111-1111-111111111111"),
                Name = Name.Create("Eagle").Value,
                WingspanInCentimeters = 200u
            },
            new
            {
                Id = Guid.Parse("b2e2e2e2-2222-2222-2222-222222222222"),
                Name = Name.Create("Parrot").Value,
                WingspanInCentimeters = 25u
            },
            new
            {
                Id = Guid.Parse("c3e3e3e3-3333-3333-3333-333333333333"),
                Name = Name.Create("Penguin").Value,
                WingspanInCentimeters = 30u
            }
        );
    }
}

internal sealed class DogSeedData : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.HasData(
            new
            {
                Id = Guid.Parse("d4e4e4e4-4444-4444-4444-444444444444"),
                Name = Name.Create("Rover").Value,
                Colour1 = Colour.Create("Brown").Value,
                Colour2 = (string?)null,
                Pattern = (string?)null
            },
            new
            {
                Id = Guid.Parse("e5e5e5e5-5555-5555-5555-555555555555"),
                Name = Name.Create("Spot").Value,
                Colour1 = Colour.Create("White").Value,
                Colour2 = Colour.Create("Black").Value,
                Pattern = Description.Create("Spots").Value
            }
        );
    }
}

internal sealed class CatSeedData : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        builder.HasData(
            new
            {
                Id = Guid.Parse("f6e6e6e6-6666-6666-6666-666666666666"),
                Name = Name.Create("Whiskers").Value,
                Colour = Description.Create("Tabby").Value
            },
            new
            {
                Id = Guid.Parse("a7e7e7e7-7777-7777-7777-777777777777"),
                Name = Name.Create("Toby").Value,
                Colour = Description.Create("Black and White").Value
            }
        );
    }
}



