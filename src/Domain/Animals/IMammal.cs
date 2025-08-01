using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Animals;

/// <remarks>
/// AnimalEntity is abstract because it has shared functionality for all animals (name).
/// IMammal is an interface because defines a contract but the underlying functionality for cat and dog is different.
/// </remarks>
public interface IMammal : IAnimal
{
    NonEmptyString HairColourDescription();
}
