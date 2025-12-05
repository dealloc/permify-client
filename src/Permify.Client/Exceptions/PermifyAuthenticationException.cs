namespace Permify.Client.Exceptions;

/// <summary>
/// Thrown when making a request with missing or invalid credentials to an endpoint that requires authentication.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception or a null reference if no inner exception is specified.</param>
/// <remarks>
/// This correlates with gRPC Unauthenticated (16) and HTTP 401.
/// </remarks>
public class PermifyAuthenticationException(
    string message,
    Exception innerException
) : PermifyClientException(message, innerException);