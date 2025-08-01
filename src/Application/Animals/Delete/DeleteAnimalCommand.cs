using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;

namespace Application.Animals.Delete;

public sealed record DeleteAnimalCommand(string AnimalType, Guid AnimalId) : ICommand;
