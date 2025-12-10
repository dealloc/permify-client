using System.Runtime.Serialization;

using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Permission;

/// <summary>
/// Response for <see cref="IPermissionService.CheckAccessControlAsync"/>.
/// </summary>
/// <param name="Can">The result of the check.</param>
/// <param name="Metadata">Additional metadata about the check(s) performed.</param>
public record CheckAccessControlResponse(
    CheckAccessControlResponse.CheckResult Can,
    CheckAccessControlResponse.ResponseMetadata Metadata
)
{
    /// <summary>
    /// Additional metadata about the check(s) performed.
    /// </summary>
    /// <param name="CheckCount">The count of the checks performed.</param>
    public sealed record ResponseMetadata(
        int CheckCount
    );

    /// <summary>
    /// Enumerates results of a check operation.
    /// </summary>
    public enum CheckResult
    {
        /// <summary>
        /// Not specified check result. This is the default value.
        /// </summary>
        [EnumMember(Value = "CHECK_RESULT_UNSPECIFIED")]
        CHECK_RESULT_UNSPECIFIED,

        /// <summary>
        /// Represents a successful check (the check allowed the operation).
        /// </summary>
        [EnumMember(Value = "CHECK_RESULT_ALLOWED")]
        CHECK_RESULT_ALLOWED,

        /// <summary>
        /// Represents a failed check (the check denied the operation).
        /// </summary>
        [EnumMember(Value = "CHECK_RESULT_DENIED")]
        CHECK_RESULT_DENIED
    }
};