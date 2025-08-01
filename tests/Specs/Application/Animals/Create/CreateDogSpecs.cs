using Application.Abstractions.Data;
using Application.Animals.Create;
using Domain.Animals;
using Domain.ValueObjects;
using FluentValidation.TestHelper;
using NSubstitute;
using SharedKernel;
using Shouldly;

namespace Specs.Application.Animals.Create;

public class CreateDogSpecs
{

    [Fact]
    public async Task Handler_Returns_Successful_Result_With_Id_With_CreateDogCommand_Containing_Valid_Required_Values_Only()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateDogCommandHandler(dbContext);
        var command = new CreateDogCommand(new string('x', Name.MaxLength), new string('x', Colour.MaxLength), null, null);

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeOfType<Guid>();
    }

    [Fact]
    public async Task Handler_Returns_Successful_Result_With_Id_With_CreateDogCommand_Containing_Valid_Required_And_Optional_Values()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateDogCommandHandler(dbContext);
        var command = new CreateDogCommand(
            new string('x', Name.MaxLength), 
            new string('x', Colour.MaxLength), 
            new string('x', Colour.MaxLength), 
            new string('x', Description.MaxLength));

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeOfType<Guid>();
    }

    // TODO: Validate all non-happy paths of CreateDogCommand containing invalid values

    /* InMemory EF DB Value converters not working
    [Fact]
    public async Task Validator_Returns_Error_When_Name_Already_Exists()
    {
        // Arrange
        string existingName = "ExistingName";
        Colour validColour = Colour.Create(new string('x', Colour.MaxLength)).Value;
        using var dbContext = TestDbContext.Create();
        dbContext.Dogs.Add(Dog.Create(Name.Create(existingName).Value, validColour, null, null));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateDogCommandValidator(dbContext);
        var command = new CreateDogCommand(existingName, validColour, null, null);

        // Act
        TestValidationResult<CreateDogCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validator_Returns_No_Error_When_Name_Is_Unique()
    {
        // Arrange
        Colour validColour = Colour.Create(new string('x', Colour.MaxLength)).Value;
        using var dbContext = TestDbContext.Create();
        dbContext.Dogs.Add(Dog.Create(Name.Create("ExistingName").Value, validColour, null, null));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateDogCommandValidator(dbContext);
        var command = new CreateDogCommand("UniqueName", validColour, null, null);

        // Act
        TestValidationResult<CreateDogCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    */
}
