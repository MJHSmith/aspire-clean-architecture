using System.Drawing;
using System.Xml.Linq;
using SharedKernel;

namespace Domain.ValueObjects;

/// <remarks>
/// Might want to consider making readonly record struct
/// + reduce garbage collection
/// - no inheritance: will have to use composition
/// - structs always have an empty constructor: will have to create public ctor that cast string.Empty to NonEmptyString to ensure failure
/// - structs can be created with default keyword (without calling any ctor): can overcome with method called in prop init - but relies on developer to call it :/
/// </remarks>
public sealed record Colour : NonEmptyString
{
    public const int MaxLength = 10;

    private Colour(NonEmptyString colour) : base(colour) { }

    public override string ToString() => Value;

    private static Result<Colour> ValidateAndCreate(NonEmptyString colour)
    {
        if (colour.Value.Length > MaxLength)
        {
            return Result.Failure<Colour>(Error.TooLong(MaxLength, colour));
        }

        return Result.Success(new Colour(colour));
    }

    public static new Result<Colour> Create(string colour)
    {
        return NonEmptyString.Create(colour).Match(
            onSuccess: ValidateAndCreate,
            onFailure: Result.Failure<Colour>
        );
    }
    
    // Helper to lift nullable string to Result<Colour?>
    public static Result<Colour?> CreateOptionalColour(string? colour) =>
        colour is null ? Result.Success<Colour?>(null) : Colour.Create(colour).Map(c => (Colour?)c);
}
