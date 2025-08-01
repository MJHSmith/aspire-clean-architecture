using Application.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using SharedKernel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Abstractions.Behaviors;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators,
        TracerProvider tracerProvider)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using TelemetrySpan span = tracerProvider.GetTracer("Web.Api")  // consider using options pattern to give application name to this layer
                .StartActiveSpan($"APPLICATION cqrs validation"); 

            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);
            if (validationFailures.Length == 0)
            {
                span.SetAttribute("validation.success", true);
                return await innerHandler.Handle(command, cancellationToken);
            }

            span.SetAttribute("validation.success", false);
            span.SetAttribute("validation.failures", validationFailures.Length);
            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators,
        TracerProvider tracerProvider)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using TelemetrySpan span = tracerProvider.GetTracer("ValidationDecorator").StartActiveSpan($"ValidationDecorator.Handle:{GetType().Name}");

            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
            {
                span.SetAttribute("validation.success", true);
                return await innerHandler.Handle(command, cancellationToken);
            }

            span.SetAttribute("validation.success", false);
            span.SetAttribute("validation.failures", validationFailures.Length);
            return Result.Failure(CreateValidationError(validationFailures));
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    private static ValidationError CreateValidationError(ValidationFailure[] validationFailures)
        => new(
            validationFailures
                .Select(f => Error.Validation(f.ErrorCode, $"{f.PropertyName}: {f.ErrorMessage}"))
                .ToArray());
}
