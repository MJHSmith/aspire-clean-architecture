using Domain.Animals;
using Domain.ValueObjects;
using SharedKernel;
using Shouldly;
using Xunit;

namespace Specs.Domain.Animals;

public class AnimalEntitySpecs
{
    [Fact]
    public void Cat_Is_Animal_And_Mammal()
    {
        // Arrange
        Name name = Name.Create("Whiskers").Value;
        Description colour = Description.Create("Tabby").Value;

        // Act
        var cat = Cat.Create(name, colour);

        // Assert
        cat.ShouldBeAssignableTo<IAnimal>();
        cat.ShouldBeAssignableTo<IMammal>();
        cat.Name.Value.ShouldBe("Whiskers");
        cat.HairColourDescription().Value.ShouldBe("Tabby");
        cat.MakeSound().ShouldBe("Meow!");
    }

    [Fact]
    public void Dog_Is_Animal_And_Mammal()
    {
        // Arrange
        Name name = Name.Create("Fido").Value;
        Colour colour1 = Colour.Create("Brown").Value;
        Colour colour2 = Colour.Create("White").Value;
        Description pattern = Description.Create("Spotted").Value;

        // Act
        var dog = Dog.Create(name, colour1, colour2, pattern);

        // Assert
        dog.ShouldBeAssignableTo<IAnimal>();
        dog.ShouldBeAssignableTo<IMammal>();
        dog.Name.Value.ShouldBe("Fido");
        dog.HairColourDescription().Value.ShouldContain("Brown");
        dog.HairColourDescription().Value.ShouldContain("White");
        dog.HairColourDescription().Value.ShouldContain("Spotted");
        dog.MakeSound().ShouldBe("Woof!");
    }

    [Fact]
    public void Bird_Is_Animal_But_Not_Mammal()
    {
        // Arrange
        Name name = Name.Create("Tweety").Value;
        uint wingspan = 25;

        // Act
        var bird = Bird.Create(name, wingspan);

        // Assert
        bird.ShouldBeAssignableTo<IAnimal>();
        bird.ShouldNotBeAssignableTo<IMammal>();
        bird.Name.Value.ShouldBe("Tweety");
        bird.WingspanInCentimeters.ShouldBe((uint)25);
        bird.MakeSound().ShouldBe("Tweet!");
    }

    [Fact]
    public void All_Animals_Make_Different_Sounds()
    {
        // Arrange
        var cat = Cat.Create(Name.Create("C").Value, Description.Create("D").Value);
        var dog = Dog.Create(Name.Create("D").Value, Colour.Create("Brown").Value);
        var bird = Bird.Create(Name.Create("B").Value, 10);

        // Act
        string[] sounds = new[] { cat.MakeSound(), dog.MakeSound(), bird.MakeSound() };

        // Assert
        sounds.Distinct().Count().ShouldBe(3);
    }
}

