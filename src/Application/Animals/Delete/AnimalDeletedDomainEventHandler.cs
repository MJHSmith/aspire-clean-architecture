using Domain.Animals;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Animals.Delete;
internal sealed class AnimalDeletedDomainEventHandler : IDomainEventHandler<AnimalDeletedDomainEvent>
{
    private readonly ILogger<AnimalDeletedDomainEventHandler> logger;

    public AnimalDeletedDomainEventHandler(ILogger<AnimalDeletedDomainEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(AnimalDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // Domain event handlers could create emails, put messages on a queue, etc.
        logger.LogWarning(
            "Animal deleted: Type={AnimalType}, Name={AnimalName}, DeletedBy={UserId}",
            domainEvent.AnimalType,
            domainEvent.AnimalName,
            domainEvent.UserId
        );

        return Task.CompletedTask;
    }
}
