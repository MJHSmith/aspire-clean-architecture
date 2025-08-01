using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Animals.Create;

public class CreateCatCommandValidator : AbstractValidator<CreateCatCommand>
{
    public CreateCatCommandValidator(IReadOnlyApplicationDbContext context)
    {
        // Although the value objects have inbuilt validation, that is more of a fail-safe,
        // so we validate here (but using the domain-layer value object constructors)
        // which gives benefit of
        //  1. multiple errors can be returned at once,
        //  2. more descriptive error messages specific to the command.

        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop) // no need to check uniqueness if invalid name
            .MustBeSuccessResult(Name.Create) // Use validation in domain layer
            .MustAsync(async (name, cancellationToken) =>
                    !await context.Cats.AnyAsync(c => c.Name == name, cancellationToken))
                .WithErrorCode("Name.MustBeUnique")
                .WithMessage("A cat with this name already exists.");

        // Use validation in domain layer
        RuleFor(c => c.HairColourDescription).MustBeSuccessResult(Description.Create); 
    }
}
