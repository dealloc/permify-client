using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Request for <see cref="IDataService.DeleteDataAsync" />
/// </summary>
/// <param name="TupleFilter">A <see cref="TupleFilter" /> to send along with the request.</param>
/// <param name="AttributeFilter">A <see cref="AttributeFilter" /> to send along with the request.</param>
public sealed record DeleteDataRequest(
    TupleFilter TupleFilter,
    AttributeFilter AttributeFilter
);