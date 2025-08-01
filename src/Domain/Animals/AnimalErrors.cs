using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKernel;

namespace Domain.Animals;
public static class AnimalErrors
{
    public static Error AlreadyTerminated(Guid animalId) => Error.Problem(
        "Animals.AlreadyTerminated",
        $"The animal item with Id = '{animalId}' is already terminated.");

    public static Error NotFound(Guid animalId) => Error.NotFound(
        "Animals.NotFound",
        $"The animal with the Id = '{animalId}' was not found");

    public static Error UnknownType(string animalType) => Error.Problem(
        "Animal.UnknownType", 
        $"The animal type {animalType} is unknown.");
}
