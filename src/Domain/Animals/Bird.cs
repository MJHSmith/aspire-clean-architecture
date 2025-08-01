using Domain.ValueObjects;

namespace Domain.Animals;

public sealed class Bird : AnimalEntity
{
    // To enforce non-zero, create a new ValueObject, e.g. Wingspan(applicaiton-layer)->PositiveInteger(SharedKernal)
    public uint WingspanInCentimeters { get; private set; }

    // EF Core requires a parameterless constructor
    private Bird() : base() { }

    private Bird(Name name, uint wingspanInCentimeters) : base(name) {
        WingspanInCentimeters = wingspanInCentimeters;
    }

    public static Bird Create(Name name, uint wingspanInCentimeters)
    {
        var bird = new Bird(name, wingspanInCentimeters);
        bird.Raise(new AnimalCreatedDomainEvent(bird));
        return bird;
    }

    public override string MakeSound()
    {
        return "Tweet!";
    }
}
