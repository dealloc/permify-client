namespace Permify.Client.Exceptions;

/// <summary>
/// Thrown when the Permify server returns an internal error.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception or a null reference if no inner exception is specified.</param>
/// <remarks>
/// This correlates with gRPC Internal (13) and HTTP 500.
/// </remarks>
public class PermifyInternalException(
    string message,
    Exception innerException
) : PermifyClientException(message, innerException);