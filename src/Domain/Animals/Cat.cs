using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Animals;

public sealed class Cat : AnimalEntity, IMammal
{
    public Description Colour { get; private set; }

    // EF Core requires a parameterless constructor
    private Cat() : base() { }

    private Cat(Name name, Description hairColorDescription) : base(name) 
    { 
        Colour = hairColorDescription;
    }

    /// <remarks>
    /// Only cat is responsible for creating iteslf (private constructor).
    /// If we allowed other clases to create a Cat, they might not perform correct validation or raise domain events.
    /// </remarks>
    public static Cat Create(Name name, Description hairColorDescription)
    {
        var cat = new Cat(name, hairColorDescription);
        cat.Raise(new AnimalCreatedDomainEvent(cat));
        return cat;
    }

    public NonEmptyString HairColourDescription()
    {
        return Colour;
    }

    public override string MakeSound()
    {
        return "Meow!";
    }
}
