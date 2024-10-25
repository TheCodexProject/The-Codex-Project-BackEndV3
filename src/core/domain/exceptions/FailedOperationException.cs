namespace domain.exceptions;

/// <summary>
/// An exception that is thrown when an operation fails.
/// </summary>
public class FailedOperationException : Exception
{
    /// <summary>
    /// Used for custom messages.
    /// </summary>
    /// <param name="message">Customized message.</param>
    public FailedOperationException(string message) : base(message) { }

    /// <summary>
    /// Used for inner exceptions (Like when an exception is thrown inside another exception)
    /// </summary>
    /// <param name="message">Customized message.</param>
    /// <param name="innerException">Inner exception.</param>
    public FailedOperationException(string message, Exception innerException) : base(message, innerException) { }

}