using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel;
public record NonEmptyString
{
    public string Value { get; }

    protected NonEmptyString(string value)
    {
        Value = (!string.IsNullOrWhiteSpace(value)) 
            ? value.Trim() 
            : throw new ArgumentException("Value must be non-empty.", nameof(value));
    }

    public static implicit operator string(NonEmptyString value) => value.Value;

    /// <reamrks>
    /// Don't want implicit, because not all strings are non-empty, but allow explicit conversions.
    /// </remarks>
    public static explicit operator NonEmptyString(string value) => new NonEmptyString(value);

    public override string ToString() => Value;

    public static Result<NonEmptyString> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<NonEmptyString>(Error.EmptyOrNullValue);
        }

        return Result.Success(new NonEmptyString(value.Trim()));
    }
}

