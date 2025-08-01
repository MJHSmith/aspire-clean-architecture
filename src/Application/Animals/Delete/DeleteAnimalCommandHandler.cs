using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Animals.Delete;

internal sealed class DeleteAnimalCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<DeleteAnimalCommand>
{
    public async Task<Result> Handle(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        if(!Enum.TryParse(command.AnimalType, ignoreCase: true, out AnimalType animalType))
        {
            return Result.Failure(AnimalErrors.UnknownType(command.AnimalType));
        }

        // check command.AnimalType, get animal from appropriate DbSet based on type
        // and delete it if found, otherwise return NotFound error
        AnimalEntity? animal = animalType switch
        {
            AnimalType.Bird =>
                await context.Birds
                    .Where(b => b.Id == command.AnimalId)
                    .FirstOrDefaultAsync(cancellationToken),
            AnimalType.Cat =>
                await context.Cats
                    .Where(c => c.Id == command.AnimalId)
                    .FirstOrDefaultAsync(cancellationToken),
            AnimalType.Dog =>
                await context.Dogs
                    .Where(d => d.Id == command.AnimalId)
                    .FirstOrDefaultAsync(cancellationToken),
            _ => null
        };

        if (animal is null)
        {
            return Result.Failure(AnimalErrors.NotFound(command.AnimalId));
        }
        
        switch (animalType)
        {
            case AnimalType.Bird:
                context.Birds.Remove((Bird)animal);
                break;
            case AnimalType.Cat:
                context.Cats.Remove((Cat)animal);
                break;
            case AnimalType.Dog:
                context.Dogs.Remove((Dog)animal);
                break;
            default:
                return Result.Failure(AnimalErrors.UnknownType(command.AnimalType.ToString()));
        }

        animal.Raise(new AnimalDeletedDomainEvent(command.AnimalType.ToString(), animal.Name, userContext.UserId));
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
