using Application.Abstractions.Messaging;

namespace Application.Animals.Create;

public readonly record struct CreateBirdCommand
(
    string Name,
    uint WingspanInCentimeters
) : ICommand<Guid> { }
