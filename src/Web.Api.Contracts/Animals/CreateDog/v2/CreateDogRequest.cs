using System.ComponentModel.DataAnnotations;

namespace Web.Api.Contracts.Animals.CreateDog.v2;

/// <summary>
/// The v2 request used to create a dog.
/// </summary>
/// <remarks>
/// Avoid using primary constructor because Open API does not support it.
/// </remarks>
public readonly record struct CreateDogRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDogRequest"/> class.
    /// </summary>
    public CreateDogRequest()
    {
    }

    /// <summary>
    /// The name of the dog.
    /// </summary>
    /// <example>Buddy</example>
    [Required]
    public string Name { get; init; } = default!;

    /// <summary>
    /// Gets the primary hair color of the dog.
    /// </summary>
    /// <example>Brown</example>
    [Required]
    public string HairColour1 { get; init; } = default!;

    /// <summary>
    /// Gets the secondary hair color of the dog.
    /// </summary>
    /// <example>Black</example>
    public string? HairColour2 { get; init; }

    /// <summary>
    /// Gets the hair pattern description, which may include details such as texture, style, or other characteristics.
    /// </summary>
    /// <example>Patches</example>
    public string? HairPattern { get; init; }
}
