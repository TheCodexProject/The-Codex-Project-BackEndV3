namespace OperationResult;

/// <summary>
/// A class that represents the result of an operation without a return value.
/// </summary>
public class Result
{
    /// <summary>
    /// The flag that indicates if the result is a failure.
    /// (Default value is false)
    /// </summary>
    private bool  _isFailure;

    /// <summary>
    /// The collection of errors that have occurred.
    /// </summary>
    private Exception[] _errors = [];

    /// <summary>
    /// Factory method to create a new instance of Result that is a success.
    /// </summary>
    /// <returns>A successful <see cref="Result"/></returns>
    public static Result Success() => new();

    /// <summary>
    /// Factory method to create a new instance of Result that is a failure.
    /// </summary>
    /// <param name="errors">An array of errors that has happened.</param>
    /// <returns>A failed <see cref="Result"/></returns>
    public static Result Failure(params Exception[] errors) => new() { _isFailure = true, _errors = errors };

    /// <summary>
    /// A flag that indicates if the result is a failure.
    /// </summary>
    public bool IsFailure => _isFailure;

    /// <summary>
    /// A flag that indicates if the result is a success.
    /// </summary>
    public bool IsSuccess => !_isFailure;

    /// <summary>
    /// The collection of errors that have occurred.
    /// </summary>
    public IEnumerable<Exception> Errors => _errors;
}

/// <summary>
/// A class that represents the result of an operation with a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T>
{
    /// <summary>
    /// The collection of errors that have occurred.
    /// </summary>
    private Exception[] _errors = [];

    /// <summary>
    /// A value that is returned from the operation
    /// </summary>
    public T Value { get; private init; } = default!;

    /// <summary>
    /// Implicit conversion operator to convert a T to a Result.
    /// </summary>
    /// <param name="value">The value to be returned.</param>
    /// <returns>Returns a sucessful <see cref="Result"/> with a return false. </returns>
    public static implicit operator Result<T>(T value) => Success(value);

    /// <summary>
    /// Implicit conversion operator to convert a Result to a T.
    /// </summary>
    /// <param name="result">The result to be converted</param>
    /// <returns>The value from within the <see cref="Result"/></returns>
    public static implicit operator T(Result<T> result) => result.Value;

    /// <summary>
    /// Factory method to create a new instance of Result that is a success.
    /// </summary>
    /// <param name="value">The value to be returned.</param>
    /// <returns>A sucessful <see cref="Result"/> with a return value.</returns>
    public static Result<T> Success(T value) => new() { Value = value };

    /// <summary>
    /// Factory method to create a new instance of Result that is a failure.
    /// </summary>
    /// <param name="errors">A collection of errors that has occured.</param>
    /// <returns>A failed <see cref="Result"/></returns>
    public static Result<T> Failure(params Exception[] errors) => new() { IsFailure = true, _errors = errors };

    /// <summary>
    /// A flag that indicates if the result is a failure.
    /// </summary>
    public bool IsFailure { get; private set; }

    /// <summary>
    /// A flag that indicates if the result is a success.
    /// </summary>
    public bool IsSuccess => !IsFailure;

    /// <summary>
    /// The collection of errors that have occurred.
    /// </summary>
    public IEnumerable<Exception> Errors => _errors;
}