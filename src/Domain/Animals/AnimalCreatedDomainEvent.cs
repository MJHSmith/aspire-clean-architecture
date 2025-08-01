using SharedKernel;

namespace Domain.Animals;

public sealed record AnimalCreatedDomainEvent(IAnimal animal) : IDomainEvent;
