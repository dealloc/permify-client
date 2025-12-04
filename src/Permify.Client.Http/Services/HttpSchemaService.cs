using Permify.Client.Contracts;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models.Schema;
using Permify.Client.Models.Schema;

namespace Permify.Client.Http.Services;

/// <summary>
/// Implements <see cref="ISchemaService"/> using HTTP.
/// </summary>
internal sealed class HttpSchemaService(ApiClient api) : ISchemaService
{
    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await api.V1.Tenants["t1"].Schemas.Write
            .PostAsync(new WriteBody { Schema = request.Schema }, cancellationToken: cancellationToken);

        // TODO: handle nullability.
        return new WriteSchemaResponse(response!.SchemaVersion!);
    }

    /// <inheritdoc />
    public async Task<ListSchemaResponse> ListSchemaAsync(ListSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await api.V1.Tenants["t1"].Schemas.List
            .PostAsync(new ListBody { PageSize = request.PageSize, ContinuousToken = request.ContinuousToken },
                cancellationToken: cancellationToken);

        // TODO: handle nullability.
        return new ListSchemaResponse(
            response!.Head!,
            response!.Schemas!.Select(item =>
                new ListSchemaResponse.SchemaItem(item.Version!, DateTime.Parse(item.CreatedAt!))).ToList(),
            response!.ContinuousToken!
        );
    }
}