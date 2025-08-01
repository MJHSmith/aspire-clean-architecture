namespace Application.Animals.GetAll;

public readonly record struct AnimalResponse(
    Guid Id,
    string Type,
    bool IsMammel,
    string Name,
    string Sound
){}
