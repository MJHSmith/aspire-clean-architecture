using Application.Abstractions.Messaging;

namespace Application.Animals.Create;

public readonly record struct CreateDogCommand
(
    string Name,
    string Colour1,
    string? Colour2,
    string? HairPattern
) : ICommand<Guid> { }
