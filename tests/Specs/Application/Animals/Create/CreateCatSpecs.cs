using System.Linq.Expressions;
using Application.Abstractions.Data;
using Application.Animals.Create;
using Domain.Animals;
using Domain.ValueObjects;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace Specs.Application.Animals.Create;

public class CreateCatSpecs
{
    [Fact]
    public async Task Handler_Returns_Successful_Result_With_Id_With_CreateCatCommand_Containing_Valid_Values()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCatCommandHandler(dbContext);
        var command = new CreateCatCommand(new string('x', Name.MaxLength), "Tabby");

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeOfType<Guid>();
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateCatCommand_Containing_Empty_Name()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCatCommandHandler(dbContext);
        var command = new CreateCatCommand("", "Tabby");

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("General.EmptyOrNull");
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateCatCommand_Containing_Too_Long_Name()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCatCommandHandler(dbContext);
        var command = new CreateCatCommand(new string('x', Name.MaxLength + 1), "Tabby");

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("General.TooLong");
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateCatCommand_Containing_Empty_HairColourDescription()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCatCommandHandler(dbContext);
        var command = new CreateCatCommand("CatName", "");

        // Act
        Result<Guid> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("General.EmptyOrNull");
    }

    [Fact]
    public async Task Handler_Returns_Failure_Result_With_CreateCatCommand_Containing_Too_Long_HairColourDescription()
    {
        // Arrange
        IApplicationDbContext dbContext = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCatCommandHandler(dbContext);
        var command = new CreateCatCommand("CatName", new string('x', Description.MaxLength + 1));

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
        dbContext.Cats.Add(Cat.Create(Name.Create(existingName).Value, Description.Create("Tabby").Value));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateCatCommandValidator(dbContext);
        var command = new CreateCatCommand(existingName, "Tabby");

        // Act
        TestValidationResult<CreateCatCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validator_Returns_No_Error_When_Name_Is_Unique()
    {
        // Arrange
        using var dbContext = TestDbContext.Create();
        dbContext.Cats.Add(Cat.Create(Name.Create("ExistingName").Value, Description.Create("Tabby").Value));
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var validator = new CreateCatCommandValidator(dbContext);
        var command = new CreateCatCommand("UniqueName", "Tabby");

        // Act
        TestValidationResult<CreateCatCommand> result = await validator.TestValidateAsync(command, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    */
}
