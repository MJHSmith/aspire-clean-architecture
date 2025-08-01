using System.Xml.Linq;

namespace SharedKernel;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = Error.Failure(
        "General.Null",
        "Null value was provided");

    public static readonly Error EmptyOrNullValue = Error.Problem(
        "General.EmptyOrNull",
        "Empty or Null value was provided");

    public static Error TooLong(int limit, string value) => Error.Problem(
        "General.TooLong",
        $"Value can only be {limit} characters long, " +
        $"\"{value}\" is {value.Length} characters long.");

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
}
