using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Animals;
using Domain.ValueObjects;
using SharedKernel;

namespace Application.Animals.Create;

internal sealed class CreateCatCommandHandler(
    //TimeProvider timeProvider,
    IApplicationDbContext context)
    : ICommandHandler<CreateCatCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCatCommand command, CancellationToken cancellationToken)
    {
        return await Name.Create(command.Name)
            .Bind(name => Description.Create(command.HairColourDescription)
                .Bind<Description, Cat>(description => Cat.Create(name, description)))
            .BindAsync(async cat =>
            {
                context.Cats.Add(cat);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(cat.Id);
            });
 
        #region Less functional approach:
        /*
        Result<Name> nameResult = Name.Create(command.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Guid>(nameResult.Error);
        }

        Result<Description> descriptionResult = Description.Create(command.HairColourDescription);
        if (descriptionResult.IsFailure)
        {
            return Result.Failure<Guid>(descriptionResult.Error);
        }

        Result<Cat> createCatResult = Cat.Create(nameResult.Value, descriptionResult.Value); // TODO: add createdBy, and date create
        return await createCatResult.Match(
            onSuccess: async validCat => 
            {
                context.Cats.Add(validCat);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(validCat.Id);
            },
            onFailure: error => Task.FromResult(Result.Failure<Guid>(error))
        );
        */
        #endregion
    }
}
