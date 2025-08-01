using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Animals;
using SharedKernel;

namespace Domain.ValueObjects;

public sealed record Name : NonEmptyString
{
    public const int MaxLength = 15;

    private Name(NonEmptyString name) : base(name) { }

    public override string ToString() => Value;

    private static Result<Name> ValidateAndCreate(NonEmptyString name)
    {
        if (name.Value.Equals("Bob", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure<Name>(NameErrors.CannotBeCalledBob);
        }

        if (!name.Value.All(char.IsLetter))
        {
            string invalidChars = new string([.. name.Value.Where(c => !char.IsLetter(c))]);
            return Result.Failure<Name>(NameErrors.InvalidCharacters(invalidChars));
        }

        if (name.Value.Length > MaxLength) // NOTE: MaxLength repeated in many value objects, consider MaxLengthNonEmptyString record
        {
            return Result.Failure<Name>(Error.TooLong(MaxLength, name));
        }

        return Result.Success(new Name(name));
    }

    public static new Result<Name> Create(string name)
    {
        return NonEmptyString.Create(name).Match(
            onSuccess: ValidateAndCreate,
            onFailure: Result.Failure<Name>
        );
    }
}

/*
/// <remarks>
/// Get to keep primary contructor, but must throw exceptions instead of using result pattern.
/// </remarks>
public sealed record NameWithCtorValidation(NonEmptyString Name) : NonEmptyString(Validate(Name))
{
    public const int MaxLength = 15;

    public NameWithCtorValidation(string name) : this((NonEmptyString)name) { }

    private static string Validate(NonEmptyString name)
    {
        if (name.Value.Equals("Bob", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("No animal may be called Bob.", nameof(name));
        }

        if (!name.Value.All(char.IsLetter))
        {
            throw new ArgumentException("Name can only contain letters.", nameof(name));
        }

        if (name.Value.Length > MaxLength)
        {
            throw new ArgumentException($"Name cannot be longer than {MaxLength} characters.", nameof(name));
        }

        return name.Value;
    }
}

/// <remarks>
/// Need more boilerplate previous provided by record, but creation using result pattern.
/// </remarks>
public sealed record NameWithPrivateCtor : NonEmptyString
{
    public const int MaxLength = 15;

    private NameWithPrivateCtor(NonEmptyString name) : base(name) { }

    private static Result<NameWithPrivateCtor> ValidateAndCreate(NonEmptyString name)
    {
        if (name.Value.Equals("Bob", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure<NameWithPrivateCtor>(NameErrors.CannotBeCalledBob);
        }

        if (!name.Value.All(char.IsLetter))
        {
            string invalidChars = new string ([.. name.Value.Where(c => !char.IsLetter(c))]);
            return Result.Failure<NameWithPrivateCtor>(NameErrors.InvalidChacters(invalidChars));
        }

        if (name.Value.Length > MaxLength)
        {
            return Result.Failure<NameWithPrivateCtor>(NameErrors.TooLong(name));
        }

        return Result.Success(new NameWithPrivateCtor(name));
    }

    public static new Result<NameWithPrivateCtor> Create(string name)
    {
        return NonEmptyString.Create(name).Match(
            onSuccess: ValidateAndCreate,
            onFailure: Result.Failure<NameWithPrivateCtor>
        );
    }
}
*/
