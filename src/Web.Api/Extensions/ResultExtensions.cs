using SharedKernel;

namespace Web.Api.Extensions;

/// <summary>
/// Provides extension methods for matching on <see cref="Result"/> and <see cref="Result{T}"/>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Matches on a <see cref="Result"/> and executes the corresponding function based on success or failure.
    /// </summary>
    /// <typeparam name="TOut">The return type.</typeparam>
    /// <param name="result">The result to match on.</param>
    /// <param name="onSuccess">Function to execute if the result is successful.</param>
    /// <param name="onFailure">Function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    /// <summary>
    /// Matches on a <see cref="Result{TIn}"/> and executes the corresponding function based on success or failure.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
    /// <typeparam name="TOut">The return type.</typeparam>
    /// <param name="result">The result to match on.</param>
    /// <param name="onSuccess">Function to execute if the result is successful.</param>
    /// <param name="onFailure">Function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
