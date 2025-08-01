using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Animals;

/// <remarks>
/// AnimalEntity is abstract because it has shared functionality for all animals (name).
/// IMammal is an interface because defines a contract but the underlying functionality is different.
/// </remarks>
public abstract class AnimalEntity : Entity, IAnimal
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "'private set' added for EF core seeding via migrations.")]
    public Guid Id { get; private set; }
    public Name Name { get; private set; }

    /*
    public DateTimeOffset  CreatedAt { get; private set; }
    public DateTimeOffset? DateTerminated { get; private set; }
    public State State { get; private set; }
    public Result Terminate(TimeProvider timeProvider)
    {
        if (State == State.Dead)
        {
            return Result.Failure(AnimalErrors.AlreadyTerminated(Id));
        }
        DiedAt = timeProvider.UtcNow;
        State = State.Dead;

        return Result.Success();
    }
   */

    // EF Core requires a parameterless constructor
    protected AnimalEntity() { }

    protected AnimalEntity(Name name)
    {
        Name = name;
    }

    public abstract string MakeSound();
}

public interface IAnimal
{
    Guid Id { get; }
    Name Name { get; }
    string MakeSound();
}

public enum State
{
    Alive = 1,
    Dead = 0
}
