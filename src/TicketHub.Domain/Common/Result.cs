namespace TicketHub.Domain.Common;

/// <summary>
/// The outcome of an operation that can fail for business reasons.
/// Domain code returns results instead of throwing exceptions.
/// </summary>
/// <typeparam name="T">The value produced on success.</typeparam>
public sealed record Result<T>
{
    private Result(T value) => Value = value;

    private Result(Error error) => Error = error;

    /// <summary>The value, present only when <see cref="IsSuccess"/> is true.</summary>
    public T? Value { get; }

    /// <summary>The error, present only when <see cref="IsFailure"/> is true.</summary>
    public Error? Error { get; }

    /// <summary>True when the operation succeeded.</summary>
    public bool IsSuccess => Error is null;

    /// <summary>True when the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>Creates a successful result.</summary>
    public static Result<T> Success(T value) => new(value);

    /// <summary>Creates a failed result.</summary>
    public static Result<T> Failure(Error error) => new(error);

    /// <summary>Transforms the value of a successful result; failures pass through unchanged.</summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> map) =>
        IsSuccess ? Result<TOut>.Success(map(Value!)) : Result<TOut>.Failure(Error!);

    /// <summary>Collapses the result into a single value.</summary>
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error!);
}
