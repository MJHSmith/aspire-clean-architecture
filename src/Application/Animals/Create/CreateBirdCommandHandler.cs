using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Domain.ValueObjects;
using SharedKernel;

namespace Application.Animals.Create;

internal sealed class CreateBirdCommandHandler(
    //TimeProvider timeProvider,
    IApplicationDbContext context)
    : ICommandHandler<CreateBirdCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBirdCommand command, CancellationToken cancellationToken)
    {
        return await Name.Create(command.Name)
            .Bind<Name, Bird>(name => Bird.Create(name, command.WingspanInCentimeters))
            .BindAsync(async bird =>
            {
                context.Birds.Add(bird);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(bird.Id);
            });

        #region Less functional approach:
        /*
        Result<Name> nameResult = Name.Create(command.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }

        Result<Bird> createBirdResult = Bird.Create(nameResult.Value, command.WingspanInCentimeters); // TODO: add createdBy, and date create
        return await createBirdResult.Match(
            onSuccess: async validBird => 
            {
                context.Birds.Add(validBird);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(validBird.Id);
            },
            onFailure: error => Task.FromResult(Result.Failure<Guid>(error))
        );
        */
        #endregion
    }
}
