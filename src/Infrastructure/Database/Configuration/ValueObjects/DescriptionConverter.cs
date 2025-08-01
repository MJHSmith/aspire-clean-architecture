using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.Configuration.ValueObjects;

public class DescriptionConverter : ValueConverter<Description, string>
{
    public DescriptionConverter() : base(
        description => description.Value,
        dbValue => dbValue == null ? null : Description.Create(dbValue).Value)
    { }
}

public class NullableDescriptionConverter : ValueConverter<Description?, string>
{
    public NullableDescriptionConverter() : base(
        description => description == null ? null : description.Value,
        dbValue => dbValue == null ? null : Description.Create(dbValue).Value)
    { }
}
