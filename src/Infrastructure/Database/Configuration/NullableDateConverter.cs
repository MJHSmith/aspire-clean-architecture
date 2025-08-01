using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;

namespace Infrastructure.Database.Configuration;

public class NullableDateConverter : ValueConverter<DateTime?, DateTime>
{
    public NullableDateConverter() : base(
        date => date.HasValue ? DateTime.SpecifyKind(date.Value, DateTimeKind.Utc) : default,
        dbValue => dbValue)
    { }
}

public class NullableDateTimeOffsetConverter : ValueConverter<DateTimeOffset?, DateTimeOffset>
{
    public NullableDateTimeOffsetConverter() : base(
        dto => dto.HasValue ? DateTime.SpecifyKind(dto.Value.DateTime, DateTimeKind.Utc) : default,
        dbValue => dbValue)
    { }
}


