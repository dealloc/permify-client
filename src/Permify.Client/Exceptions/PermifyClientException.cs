namespace Permify.Client.Exceptions;

/// <summary>
/// Base class for all Permify client exceptions.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception or a null reference if no inner exception is specified.</param>
public abstract class PermifyClientException(
    string? message = null,
    Exception? innerException = null
) : Exception(message, innerException);