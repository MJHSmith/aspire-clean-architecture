using Application.Abstractions.Data;
using Application.Animals.Create;
using Domain.Animals;
using Domain.ValueObjects;
using FluentValidation.TestHelper;
using NSubstitute;
using SharedKernel;
using Shouldly;

namespace Specs.Application.Animals.Create;

public class CreateBirdSpecs
{
    [Fact]
    public async Task Handler_Returns_Successful_Result_With_Id_With_CreateBirdCommand_Containing_Valid_Values()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateBirdCommandHandler(dbContext);
        var command = new CreateBirdCommand(new string('x', Name.MaxLength), 25);

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeOfType<Guid>();
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateBirdCommand_Containing_Empty_Name()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateBirdCommandHandler(dbContext);
        var command = new CreateBirdCommand("", 25);

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("General.EmptyOrNull");
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateBirdCommand_Containing_Too_Long_Name()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateBirdCommandHandler(dbContext);
        var command = new CreateBirdCommand(new string('x', Name.MaxLength + 1), 25);

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("General.TooLong");
    }

    /* InMemory EF DB Value converters not working
    [Fact]
    public async Task Validator_Returns_Error_When_Name_Already_Exists()
    {
        // Arrange
        string existingName = "ExistingName";
        using var dbContext = TestDbContext.Create();
        dbContext.Birds.Add(Bird.Create(Name.Create(existingName).Value, 25));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateBirdCommandValidator(dbContext);
        var command = new CreateBirdCommand(existingName, 25);

        // Act
        TestValidationResult<CreateBirdCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validator_Returns_No_Error_When_Name_Is_Unique()
    {
        // Arrange
        using var dbContext = TestDbContext.Create();
        dbContext.Birds.Add(Bird.Create(Name.Create("ExistingName").Value, 25));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateBirdCommandValidator(dbContext);
        var command = new CreateBirdCommand("UniqueName", 25);

        // Act
        TestValidationResult<CreateBirdCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    */
}
