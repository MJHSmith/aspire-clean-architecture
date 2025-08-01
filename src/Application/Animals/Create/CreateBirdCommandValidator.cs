using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Users;
using Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Animals.Create;

public class CreateBirdCommandValidator : AbstractValidator<CreateBirdCommand>
{
    public CreateBirdCommandValidator(IReadOnlyApplicationDbContext context)
    {
        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop) // no need to check uniqueness if invalid name
            .MustBeSuccessResult(Name.Create) // Use validation in domain layer
            .MustAsync(async (name, cancellationToken) =>
                    !await context.Birds.AnyAsync(c => c.Name == name, cancellationToken))
                .WithErrorCode("Name.MustBeUnique")
                .WithMessage("A bird with this name already exists.");
    }
}
