using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Animals.Create;

public class CreateDogCommandValidator : AbstractValidator<CreateDogCommand>
{
    public CreateDogCommandValidator(IReadOnlyApplicationDbContext context)
    {
        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop) // no need to check uniqueness if invalid name
            .MustBeSuccessResult(Name.Create) // Use validation in domain layers
            .MustAsync(async (name, cancellationToken) =>
                    !await context.Dogs.AnyAsync(c => c.Name == name, cancellationToken))
                .WithErrorCode("Name.MustBeUnique")
                .WithMessage("A dog with this name already exists.");

        // Use validation in domain layer
        RuleFor(c => c.Colour1).MustBeSuccessResult(Colour.Create);
        RuleFor(c => c.Colour2).MustBeSuccessResultOrNull(Colour.Create);
        RuleFor(c => c.HairPattern).MustBeSuccessResultOrNull(Description.Create);
    }
}
