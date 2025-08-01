using System.Xml.Linq;
using SharedKernel;

namespace Domain.ValueObjects;

public sealed record Description : NonEmptyString
{
    public const int MaxLength = 20;

    private Description(NonEmptyString description) : base(description) { }

    public override string ToString() => Value;

    private static Result<Description> ValidateAndCreate(NonEmptyString description)
    {
        if (description.Value.Length > MaxLength)
        {
            return Result.Failure<Description>(Error.TooLong(MaxLength, description));
        }

        return Result.Success(new Description(description));
    }

    public static new Result<Description> Create(string description)
    {
        return NonEmptyString.Create(description).Match(
            onSuccess: ValidateAndCreate,
            onFailure: Result.Failure<Description>
        );
    }

    // Helper to lift nullable string to Result<Description?>
    public static Result<Description?> CreateOptionalDescription(string? description) =>
        description is null ? Result.Success<Description?>(null) : Description.Create(description).Map(d => (Description?)d);
}
