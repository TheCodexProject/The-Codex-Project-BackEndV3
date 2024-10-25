namespace domain.exceptions;

/// <summary>
/// An exception that is thrown when a Type T is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Used for custom messages.
    /// </summary>
    /// <param name="message">Customized message.</param>
    public NotFoundException(string message) : base(message) { }

    /// <summary>
    /// Used for inner exceptions (Like when an exception is thrown inside another exception)
    /// </summary>
    /// <param name="message">Customized message.</param>
    /// <param name="innerException">Inner exception.</param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}