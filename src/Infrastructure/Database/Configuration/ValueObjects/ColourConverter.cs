using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.Configuration.ValueObjects;

public class ColourConverter : ValueConverter<Colour, string>
{
    public ColourConverter() : base(
        colour => colour.Value,
        dbValue => Colour.Create(dbValue).Value)
    { }
}

public class NullableColourConverter : ValueConverter<Colour?, string>
{
    public NullableColourConverter() : base(
        colour => colour == null ? null : colour.Value,
        dbValue => dbValue == null ? null : Colour.Create(dbValue).Value)
    { }
}
