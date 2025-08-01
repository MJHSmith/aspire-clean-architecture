using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Domain.ValueObjects;
using SharedKernel;

namespace Application.Animals.Create;

internal sealed class CreateDogCommandHandler(
    //IDateTimeProvider dateTimeProvider,
    IApplicationDbContext context)
    : ICommandHandler<CreateDogCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDogCommand command, CancellationToken cancellationToken)
    {
        return await Name.Create(command.Name)
            .Bind(name => Colour.Create(command.Colour1)
                .Bind(colour1 => Colour.CreateOptionalColour(command.Colour2)
                    .Bind(colour2 => Description.CreateOptionalDescription(command.HairPattern)
                        .Bind<Description?,Dog>(pattern => Dog.Create(name, colour1, colour2, pattern)))))
            .BindAsync(async dog =>
            {
                context.Dogs.Add(dog);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(dog.Id);
            });

        #region Less functional approach:
        /*
        Result<Name> nameResult = Name.Create(command.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }

        Result<Colour> colour1Result = Colour.Create(command.Colour1);
        if (colour1Result.IsFailure)
        {
            return Result.Failure<Guid>(colour1Result.Error);
        }

        Result<Colour?> colour2Result = Colour.CreateOptionalColour(command.Colour2);
        if (colour2Result.IsFailure)
        {
            return Result.Failure<Guid>(colour2Result.Error);
        }


        Result<Description?> descriptionResult = Description.CreateOptionalDescription(command.HairPattern);
        if (descriptionResult.IsFailure)
        {
            return Result.Failure<Guid>(descriptionResult.Error);
        }

        Result<Dog> createDogResult = Dog.Create(
            nameResult.Value,
            colour1Result.Value,
            colour2Result.Value,
            descriptionResult.Value); // TODO: add createdBy, and date create
        return await createDogResult.Match(
            onSuccess: async validDog => 
            {
                context.Dogs.Add(validDog);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(validDog.Id);
            },
            onFailure: error => Task.FromResult(Result.Failure<Guid>(error))
        );
        */
        #endregion
    }
}
