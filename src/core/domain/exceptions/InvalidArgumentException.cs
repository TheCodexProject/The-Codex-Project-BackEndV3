namespace domain.exceptions;

/// <summary>
/// An exception that is thrown when an invalid argument is passed to a method.
/// </summary>
public class InvalidArgumentException : Exception
{
    /// <summary>
    /// Used for custom messages.
    /// </summary>
    /// <param name="message">Customized message.</param>
    public InvalidArgumentException(string message) : base(message) { }
}