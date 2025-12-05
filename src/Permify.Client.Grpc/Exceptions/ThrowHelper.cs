using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

using Grpc.Core;

using Permify.Client.Exceptions;

namespace Permify.Client.Grpc.Exceptions;

/// <summary>
/// Contains methods for converting gRPC exceptions into <see cref="PermifyClientException" />s.
/// </summary>
internal static class ThrowHelper
{
    /// <summary>
    /// Allows selectively catching gRPC exceptions by only capturing those that we want to convert.
    /// </summary>
    /// <example>
    /// Here's how you can selectively only catch exceptions the <see cref="ThrowHelper" /> can convert:
    /// <code>
    /// try
    /// {
    ///     // ...
    /// }
    /// catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
    /// {
    ///     ThrowHelper.ThrowPermifyClientException(exception);
    ///     throw;
    /// }
    /// </code>
    /// </example>
    public static bool ShouldCatchException(RpcException exception) => exception.StatusCode switch
    {
        StatusCode.InvalidArgument => true,
        StatusCode.NotFound => true,
        StatusCode.Unauthenticated => true,
        StatusCode.Internal => true,
        _ => false
    };

    /// <summary>
    /// Converts a gRPC exception into a <see cref="PermifyClientException" />.
    /// If the exception cannot be converted, it will be rethrown as-is.
    /// </summary>
    /// <param name="exception">The <see cref="RpcException" /> to convert.</param>
    /// <exception cref="PermifyValidationException">Thrown for <see cref="StatusCode.InvalidArgument" />.</exception>
    /// <exception cref="PermifyNotFoundException">Thrown for <see cref="StatusCode.NotFound" />.</exception>
    /// <exception cref="PermifyAuthenticationException">Thrown for <see cref="StatusCode.Unauthenticated" />.</exception>
    /// <exception cref="PermifyInternalException">Thrown for <see cref="StatusCode.Internal" />.</exception>
    /// <seealso cref="ShouldCatchException" />
#if NET5_0_OR_GREATER
    [StackTraceHidden, DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    public static void ThrowPermifyClientException(RpcException exception)
    {
        switch (exception.StatusCode)
        {
            case StatusCode.InvalidArgument:
                throw new PermifyValidationException(exception.Status.Detail, exception);
            case StatusCode.NotFound:
                throw new PermifyNotFoundException(exception.Status.Detail, exception);
            case StatusCode.Unauthenticated:
                throw new PermifyAuthenticationException(exception.Status.Detail, exception);
            case StatusCode.Internal:
                throw new PermifyInternalException(exception.Status.Detail, exception);
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}