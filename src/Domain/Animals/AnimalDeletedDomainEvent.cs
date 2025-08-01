using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Animals;

public sealed record AnimalDeletedDomainEvent(string AnimalType, Name AnimalName, Guid UserId) : IDomainEvent;
