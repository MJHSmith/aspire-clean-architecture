using System;
using Application.Animals;
using Application.Animals.Get;
using Application.Animals.GetAll;
using Shouldly;
using Xunit;

namespace Specs.Application.Animals;

public class AnimalSpecs
{
    [Fact]
    public void AnimalType_Enum_Contains_Only_Cat_Dog_Bird()
    {
        // Arrange & Act
        string[] values = Enum.GetNames<AnimalType>();

        // Assert
        values.ShouldContain("Cat");
        values.ShouldContain("Dog");
        values.ShouldContain("Bird");
        values.Length.ShouldBe(3);
    }
}
