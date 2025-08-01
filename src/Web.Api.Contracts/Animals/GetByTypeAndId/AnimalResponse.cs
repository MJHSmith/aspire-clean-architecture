using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Api.Contracts.Animals.GetByTypeAndId;

/// <summary>
/// Represents an animal response with details such as type, name, sound, and optional attributes.
/// </summary>
public readonly record struct AnimalResponse
{
    /// <summary>
    /// Unique identifier of the animal.
    /// </summary>
    [Required]
    public Guid Id { get; init; }

    /// <summary>
    /// Type of the animal.
    /// </summary>
    [Required]
    public string Type { get; init; }

    /// <summary>
    /// Indicates whether the animal is a mammal.
    /// </summary>
    [Required]
    public bool IsMammel { get; init; }

    /// <summary>
    /// Name of the animal.
    /// </summary>
    [Required]
    public string Name { get; init; }

    /// <summary>
    /// Sound the animal makes.
    /// </summary>
    [Required]
    public string Sound { get; init; }

    /// <summary>
    /// Description of the animal's hair color, absent if not applicable.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HairColourDescription { get; init; }

    /// <summary>
    /// Wingspan of the animal in centimeters, absent if not applicable.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public uint? WingspanInCentimeters { get; init; }
}
