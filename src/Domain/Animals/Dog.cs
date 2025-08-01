using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Animals;

public sealed class Dog : AnimalEntity, IMammal
{
    public Colour Colour1 { get; private set; }
    public Colour? Colour2 { get; private set; }
    public Description? Pattern { get; private set; }

    // EF Core requires a parameterless constructor
    private Dog() : base() { }

    private Dog(Name name, Colour colour1) : base(name) {
        Colour1 = colour1;
    }

    /// <remarks>
    /// Use Value objects to overcome primative obsession.
    /// This method signature could easily be (string, string, string, string);
    /// Without Value Objects, we might pass a colour string where a name is expected.
    /// This approach can be paticularly useful when passing Guids.
    /// 
    /// It does come at the cost of additional complexity. So should only be used appropriately.
    /// </remarks>
    public static Dog Create(Name name, Colour colour1, Colour? colour2 = null, Description? patternDescription = null)
    {
        var dog = new Dog(name, colour1)
        {
            Colour2 = colour2,
            Pattern = patternDescription
        };

        dog.Raise(new AnimalCreatedDomainEvent(dog));
        return dog;
    }

    public NonEmptyString HairColourDescription()
    {
        bool hasColour2 = Colour2 is not null;
        bool hasPattern = Pattern is not null;

        string description = (hasColour2, hasPattern) switch
        {
            (false, false) => Colour1,
            (true, false) => $"{Colour1.Value} and {Colour2}",
            (false, true) => $"{Colour1} with {Pattern}",
            (true, true) => $"{Colour1} and {Colour2} - {Pattern}"
        };
    
        return (NonEmptyString)description;
    }

    public override string MakeSound()
    {
        return "Woof!";
    }
}
