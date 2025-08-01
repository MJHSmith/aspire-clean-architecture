using Application.Abstractions.Authentication;
using Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using SharedKernel;

namespace Application;
public static class CustomValidationRules
{
    public static IRuleBuilderOptions<T, Guid> MustMatchAuthenticatedUser<T>(
        this IRuleBuilder<T, Guid> ruleBuilder, 
        IUserContext userContext)
    {
        return ruleBuilder
            .Must(userId => userId == userContext.UserId)
            .WithMessage("UserId does not match the authenticated user.");
    }

    /// <summary>
    /// Adds a rule that validates using a Result-returning function.
    /// </summary>
    public static IRuleBuilderOptionsConditions<T, string> MustBeSuccessResult<T, TValue>(
        this IRuleBuilder<T, string> ruleBuilder,
        Func<string, Result<TValue>> validatorFunc)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            validatorFunc(value).Match(
                _ => { }, // Success: do nothing
                error => context.AddFailure(
                    new ValidationFailure()
                    {
                        AttemptedValue = value,
                        ErrorCode = error.Code,
                        PropertyName = context.DisplayName,
                        ErrorMessage = error.Description,
                        Severity = Severity.Error
                    })
            );
        });
    }

    /// <summary>
    /// Adds a rule that validates using a Result-returning function, but
    /// if the value to validate is null, it is ignored (considered valid).
    /// </summary>
    public static IRuleBuilderOptionsConditions<T, string?> MustBeSuccessResultOrNull<T, TValue>(
        this IRuleBuilder<T, string?> ruleBuilder,
        Func<string, Result<TValue>> validatorFunc)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (value is null)
            {
                return;
            }

            validatorFunc(value).Match(
                _ => { }, // Success: do nothing
                error => context.AddFailure(
                    new ValidationFailure()
                    {
                        AttemptedValue = value,
                        ErrorCode = error.Code,
                        PropertyName = context.DisplayName,
                        ErrorMessage = error.Description,
                        Severity = Severity.Error
                    })
            );
        });
    }
}
