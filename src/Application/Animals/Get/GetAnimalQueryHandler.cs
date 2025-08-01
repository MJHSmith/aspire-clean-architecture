using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Animals.Get;

internal sealed class GetAnimalQueryHandler(IReadOnlyApplicationDbContext context)
    : IQueryHandler<GetAnimalQuery, AnimalResponse>
{
    public async Task<Result<AnimalResponse>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(query.AnimalType, ignoreCase: true, out AnimalType animalType))
        {
            return Result.Failure<AnimalResponse>(AnimalErrors.UnknownType(query.AnimalType));
        }

        Result<AnimalResponse> animal = animalType switch
        {
            AnimalType.Bird => 
                await context.Birds
                    .Where(b => b.Id == query.AnimalId)
                    .Select(b => MapBirdToResponse(b))
                    .FirstOrDefaultAsync(cancellationToken),

            AnimalType.Cat => 
                await context.Cats
                    .Where(c => c.Id == query.AnimalId)
                    .Select(c => MapMammalToResponse(c))
                    .FirstOrDefaultAsync(cancellationToken),

            AnimalType.Dog => 
                await context.Dogs
                    .Where(d => d.Id == query.AnimalId)
                    .Select(d => MapMammalToResponse(d))
                    .FirstOrDefaultAsync(cancellationToken),

            _ => Result.Failure<AnimalResponse>(AnimalErrors.UnknownType(query.AnimalType.ToString()))
        };

        return animal;
    }

    private static AnimalResponse MapBirdToResponse(Bird b)
    {
        return new AnimalResponse
        {
            Id = b.Id,
            Name = b.Name,
            Type = b.GetType().Name,
            IsMammel = false,
            Sound = b.MakeSound(),
            WingspanInCentimeters = b.WingspanInCentimeters
        };
    }

    private static AnimalResponse MapMammalToResponse(IMammal m)
    {
        return new AnimalResponse
        {
            Id = m.Id,
            Name = m.Name,
            Type = m.GetType().Name,
            IsMammel = true,
            Sound = m.MakeSound(),
            HairColourDescription = m.HairColourDescription().Value
        };
    }
}
