using Application.Abstractions.Messaging;

namespace Application.Animals.GetAll;

public sealed record GetAnimalsQuery() : IQuery<List<AnimalResponse>>;
