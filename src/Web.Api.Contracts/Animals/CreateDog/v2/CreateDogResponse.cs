namespace Web.Api.Contracts.Animals.CreateDog.v2;

/// <summary>
/// Response returned after creating a dog, including its unique identifier and details.
/// </summary>
public readonly record struct CreateDogResponse
{
    /// <summary>
    /// Unique identifier of the created dog.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Indicates whether the animal is a mammal.
    /// </summary>
    public bool IsMammel { get; init; }

    /// <summary>
    /// Name of the dog.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Sound the dog makes.
    /// </summary>
    public string Sound { get; init; }

    /// <summary>
    /// Description of the dog's hair color.
    /// </summary>
    public string HairColourDescription { get; init; }
}
