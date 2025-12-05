namespace Permify.Client.Exceptions;

/// <summary>
/// Thrown when the Permify service returns a validation error.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception or a null reference if no inner exception is specified.</param>
/// <remarks>
/// This correlates with gRPC InvalidArgument (3) and HTTP 400.
/// </remarks>
public class PermifyValidationException(
    string message,
    Exception innerException
) : PermifyClientException(message, innerException);