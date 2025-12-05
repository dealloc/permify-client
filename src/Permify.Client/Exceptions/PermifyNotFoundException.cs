namespace Permify.Client.Exceptions;

/// <summary>
/// Thrown when the Permify server returns a not found error.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception or a null reference if no inner exception is specified.</param>
/// <remarks>
/// This correlates with gRPC NotFound (4) and HTTP 404.
/// </remarks>
public class PermifyNotFoundException(
    string message,
    Exception innerException
) : PermifyClientException(message, innerException);