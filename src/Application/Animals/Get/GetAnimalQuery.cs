using Application.Abstractions.Data;
using Application.Abstractions.Messaging;

namespace Application.Animals.Get;

public sealed record GetAnimalQuery(string AnimalType, Guid AnimalId) : IQuery<AnimalResponse>;


