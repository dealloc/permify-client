using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Watch;

/// <summary>
/// Response for <see cref="IWatchService.WatchAsync" />.
/// </summary>
/// <param name="Changes">Represents changes in data with a snap token and a list of data change objects.</param>
public sealed record WatchResponse(
    WatchResponse.WatchDataChanges Changes
)
{
    /// <summary>
    /// Represent changes in data with a snap token and a list of data change objects.
    /// </summary>
    /// <param name="SnapToken">The snapshot token.</param>
    /// <param name="DataChanges">The list of data changes, note that it can be empty.</param>
    public sealed record WatchDataChanges(
        string SnapToken,
        IReadOnlyList<WatchDataChange>? DataChanges
    );

    /// <summary>
    /// Describes a single change in data.
    /// </summary>
    /// <param name="Operation">The <see cref="Permify.Client.Models.V1.Watch.WatchResponse.Operation" /> this change describes.</param>
    /// <param name="Tuple">If the change was to a <see cref="Permify.Client.Models.V1.Tuple" /> it's added here, otherwise this is <c>null</c>.</param>
    /// <param name="Attribute">If the change was to an <see cref="AttributeEntity" /> it's added here, otherwise this is <c>null</c>.</param>
    public sealed record WatchDataChange(
        Operation Operation,
        Tuple? Tuple,
        AttributeEntity? Attribute
    );

    /// <summary>
    /// Describes what type of change the <see cref="WatchDataChange" /> represents.
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Default operation, not specified.
        /// </summary>
        OPERATION_UNSPECIFIED,

        /// <summary>
        /// Creation operation.
        /// </summary>
        OPERATION_CREATE,

        /// <summary>
        /// Deletion operation.
        /// </summary>
        OPERATION_DELETE
    }
};