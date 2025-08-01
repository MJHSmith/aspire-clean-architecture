using System.Text.Json.Serialization;

namespace Application.Animals.Get;

public readonly partial record struct AnimalResponse(
    Guid Id,
    string Type,
    bool IsMammel,
    string Name,
    string Sound,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? HairColourDescription = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    uint? WingspanInCentimeters = null
);

