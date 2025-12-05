using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

using Permify.Client.Exceptions;
using Permify.Client.Http.Generated.Models;

namespace Permify.Client.Http.Exceptions;

/// <summary>
/// Contains methods for converting HTTP exceptions into <see cref="PermifyClientException" />s.
/// </summary>
internal static class ThrowHelper
{
    /// <summary>
    /// Allows selectively catching HTTP exceptions by only capturing those that we want to convert.
    /// </summary>
    /// <example>
    /// Here's how you can selectively only catch exceptions the <see cref="ThrowHelper" /> can convert:
    /// <code>
    /// try
    /// {
    ///     // ...
    /// }
    /// catch (Permify.Client.Http.Generated.Models.Status exception) when (ThrowHelper.ShouldCatchException(exception))
    /// {
    ///     ThrowHelper.ThrowPermifyClientException(exception);
    ///     throw;
    /// }
    /// </code>
    /// </example>
    public static bool ShouldCatchException(Status exception) => exception.Code switch
    {
        // InvalidArgument
        3 => true,
        // NotFound
        5 => true,
        // Internal
        13 => true,
        // Unauthenticated
        16 => true,
        _ => false
    };

    /// <summary>
    /// Converts an HTTP exception into a <see cref="PermifyClientException" />.
    /// If the exception cannot be converted, it will be rethrown as-is.
    /// </summary>
    /// <param name="exception">The <see cref="Status" /> too convert.</param>
    /// <exception cref="PermifyValidationException">Thrown for code <c>3</c>.</exception>
    /// <exception cref="PermifyNotFoundException">Thrown for code <c>5</c>.</exception>
    /// <exception cref="PermifyInternalException">Thrown for code <c>13</c>.</exception>
    /// <exception cref="PermifyAuthenticationException">Thrown for code <c>16</c>.</exception>
#if NET5_0_OR_GREATER
    [StackTraceHidden, DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    public static void ThrowPermifyClientException(Status exception)
    {
        switch (exception.Code)
        {
            case 3:
                throw new PermifyValidationException(exception.Message, exception);
            case 5:
                throw new PermifyNotFoundException(exception.Message, exception);
            case 13:
                throw new PermifyInternalException(exception.Message, exception);
            case 16:
                throw new PermifyAuthenticationException(exception.Message, exception);
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}