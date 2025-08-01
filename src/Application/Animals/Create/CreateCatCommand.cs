using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Animals.Create;

public readonly record struct CreateCatCommand
(
    string Name,
    string HairColourDescription
) : ICommand<Guid> { }
