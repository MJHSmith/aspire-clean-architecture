using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.Configuration.ValueObjects;

public class NameConverter : ValueConverter<Name, string>
{
    public NameConverter() : base(
        name => name.Value,
        dbValue => Name.Create(dbValue).Value)
    { }
}
