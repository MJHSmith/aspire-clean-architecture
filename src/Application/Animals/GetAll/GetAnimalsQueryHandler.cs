using System.Linq;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Animals.GetAll;

internal sealed class GetAnimalsQueryHandler(IReadOnlyApplicationDbContext context, ILogger<GetAnimalsQueryHandler> logger)
    : IQueryHandler<GetAnimalsQuery, List<AnimalResponse>>
{

    public async Task<Result<List<AnimalResponse>>> Handle(GetAnimalsQuery query, CancellationToken cancellationToken)
    {
        // Projections into AnimalResponse stops tracking by default and only pulls the data required.

        List<AnimalResponse> birds = await context.Birds
            .Select(b => new AnimalResponse
            {
                Id = b.Id,
                Name = b.Name,
                Type = b.GetType().Name,
                IsMammel = false, //b is IMammal, // always false for birds
                Sound = b.MakeSound(),
            })
            .ToListAsync(cancellationToken);

        List<AnimalResponse> cats = await context.Cats
            .Select(c => new AnimalResponse
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.GetType().Name,
                IsMammel = c is IMammal,
                Sound = c.MakeSound(),
            })
            .ToListAsync(cancellationToken);

        List<AnimalResponse> dogs = await context.Dogs
            .Select(d => new AnimalResponse
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.GetType().Name,
                IsMammel = d is IMammal,
                Sound = d.MakeSound(),
            })
            .ToListAsync(cancellationToken);

        //return birds.Concat(cats).Concat(dogs).ToList();

        #region Code to meet requirements of tech test:

        List<Bird> birdList = await context.Birds.ToListAsync(cancellationToken);
        List<Cat> catList = await context.Cats.ToListAsync(cancellationToken);
        List<Dog> dogList = await context.Dogs.ToListAsync(cancellationToken);

        // 1. All put into single array or list
        List<IAnimal> animals = [..birdList, ..catList, ..dogList];

        // 2. Loop the array and examine different types in the list
        for (int i = 0; i < animals.Count; i++)
        {
            switch (animals[i])
            {
                case Bird bird:
                    logger.LogInformation("Processing bird: {Name} ({Wingspan})", bird.Name, bird.WingspanInCentimeters);
                    break;
                case Cat cat:
                    logger.LogInformation("Processing cat: {Name} ({HairColourDescription})", cat.Name, cat.HairColourDescription());
                    break;
                case Dog dog:
                    logger.LogInformation("Processing dog: {Name} ({HairColourDescription})", dog.Name, dog.HairColourDescription());
                    break;
            }
        }

        // 3. Query the array using LINQ
        var groupedAndSorted = animals
            .GroupBy(a => a.GetType().Name)
            .Select(g => new
            {
                Type = g.Key,
                Animals = g.OrderBy(a => a.Name.ToString()).ToList()
            })
            .ToList();
        logger.LogInformation(
            "Grouped and sorted animals: {GroupedAnimals}", 
            System.Text.Json.JsonSerializer.Serialize(groupedAndSorted));
        #endregion

        return birds.Concat(cats).Concat(dogs).ToList();
    }
}
