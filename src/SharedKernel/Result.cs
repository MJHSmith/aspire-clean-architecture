using System.Diagnostics.CodeAnalysis;

namespace SharedKernel;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}


public static class ResultExtensions
{
    /// <summary>
    /// Executes one of the specified functions based on the success or failure state of the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="result">The result to evaluate, determining which function to execute.</param>
    /// <param name="onSuccess">A function to execute if the result indicates success. The function returns a value of type <typeparamref
    /// name="T"/>.</param>
    /// <param name="onFailure">A function to execute if the result indicates failure. The function takes an <see cref="Error"/> as a parameter
    /// and returns a value of type <typeparamref name="T"/>.</param>
    /// <returns>The value returned by either <paramref name="onSuccess"/> or <paramref name="onFailure"/>, depending on the
    /// state of the result.</returns>
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    /// <summary>
    /// Executes one of the specified functions based on the success or failure state of the result.
    /// </summary>
    /// <remarks>
    /// Can be thought of as a switch statement.
    /// </remarks>
    /// <typeparam name="TValue">The type of the value contained in the result if it is successful.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the functions.</typeparam>
    /// <param name="result">The result to evaluate, which determines which function to execute.</param>
    /// <param name="onSuccess">A function to execute if the result is successful, receiving the value as an argument.</param>
    /// <param name="onFailure">A function to execute if the result is a failure, receiving the error as an argument.</param>
    /// <returns>The result of the executed function, either from <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    public static TResult Match<TValue, TResult>(
        this Result<TValue> result,
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }

    /// <summary>
    /// Transforms the result of a successful operation by applying a specified function, or propagates the error if the
    /// operation failed.
    /// </summary>
    /// <remarks>
    /// Often called "Railway Track": Think of a railway track with switches.
    /// If the train (your value) derails (an error), it never reaches the next station (function); otherwise, it continues.
    /// </remarks>
    /// <typeparam name="TBefore">The type of the value contained in the initial result.</typeparam>
    /// <typeparam name="TAfter">The type of the value contained in the resulting transformed result.</typeparam>
    /// <param name="result">The initial result to transform. Must not be null.</param>
    /// <param name="func">A function to apply to the value of the result if it is successful. Must not be null.</param>
    /// <returns>A new <see cref="Result{TAfter}"/> containing the transformed value if the initial result was successful;
    /// otherwise, a <see cref="Result{TAfter}"/> containing the original error.</returns>
    public static Result<TAfter> Bind<TBefore, TAfter>(this Result<TBefore> result, Func<TBefore, Result<TAfter>> func)
    => result.IsFailure ? Result.Failure<TAfter>(result.Error) : func(result.Value);

    /// <summary>
    /// Transforms the result of a successful operation by applying a specified function, or propagates the error if the
    /// operation failed.
    /// </summary>
    /// <remarks>
    /// Often called "Railway Track": Think of a railway track with switches.
    /// If the train (your value) derails (an error), it never reaches the next station (function); otherwise, it continues.
    /// </remarks>
    /// <typeparam name="TBefore">The type of the value contained in the initial result.</typeparam>
    /// <typeparam name="TAfter">The type of the value contained in the resulting transformed result.</typeparam>
    /// <param name="result">The initial result to transform. Must not be null.</param>
    /// <param name="func">A function to apply to the value of the result if it is successful. Must not be null.</param>
    /// <returns>A new <see cref="Result{TAfter}"/> containing the transformed value if the initial result was successful;
    /// otherwise, a <see cref="Result{TAfter}"/> containing the original error.</returns>
    public static async Task<Result<TAfter>> BindAsync<TBefore, TAfter>(this Result<TBefore> result, Func<TBefore, Task<Result<TAfter>>> func)
        => result.IsFailure ? Result.Failure<TAfter>(result.Error) : await func(result.Value);

    /// <summary>
    /// Transforms the successful result of the current <see cref="Result{TCurrent}"/> into a new <see cref="Result{TNew}"/> using
    /// the specified mapping function.
    /// </summary>
    /// <typeparam name="TCurrent">The type of the value contained in the original result.</typeparam>
    /// <typeparam name="TNew">The type of the value to be contained in the transformed result.</typeparam>
    /// <param name="result">The original result to transform. Must not be null.</param>
    /// <param name="func">A function to apply to the value of the original result if it is successful. Must not be null.</param>
    /// <returns>A new <see cref="Result{TNew}"/> containing the transformed value if the original result is successful;    
    /// otherwise, a failure result with the same error as the original.</returns>
    public static Result<TNew> Map<TCurrent, TNew>(this Result<TCurrent> result, Func<TCurrent, TNew> func)
    {
        return result.IsFailure
            ? Result.Failure<TNew>(result.Error)
            : Result.Success(func(result.Value));
    }

    /// <summary>
    /// Executes one of the specified actions based on the success or failure state of the result.
    /// </summary>
    /// <param name="result">The result to evaluate, determining which action to execute.</param>
    /// <param name="onSuccess">An action to execute if the result indicates success.</param>
    /// <param name="onFailure">An action to execute if the result indicates failure. The action takes an <see cref="Error"/> as a parameter.</param>
    public static void Match(
        this Result result,
        Action onSuccess,
        Action<Error> onFailure)
    {
        if (result.IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFailure(result.Error);
        }
    }

    /// <summary>
    /// Executes one of the specified actions based on the success or failure state of the result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result if it is successful.</typeparam>
    /// <param name="result">The result to evaluate, determining which action to execute.</param>
    /// <param name="onSuccess">An action to execute if the result indicates success, receiving the value as an argument.</param>
    /// <param name="onFailure">An action to execute if the result indicates failure. The action receives the error as an argument.</param>
    public static void Match<TValue>(
        this Result<TValue> result,
        Action<TValue> onSuccess,
        Action<Error> onFailure)
    {
        if (result.IsSuccess)
        {
            onSuccess(result.Value);
        }
        else
        {
            onFailure(result.Error);
        }
    }
}
