using Base.V1;

using Permify.Client.Contracts;
using Permify.Client.Models.Schema;

namespace Permify.Client.Grpc.Services;

/// <summary>
/// Implements <see cref="ISchemaService"/> using gRPC.
/// </summary>
public sealed class GrpcSchemaService(Base.V1.Schema.SchemaClient client) : ISchemaService
{
    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await client.WriteAsync(new SchemaWriteRequest { TenantId = "t1", Schema = request.Schema },
            cancellationToken: cancellationToken).ResponseAsync;

        return new WriteSchemaResponse(response.SchemaVersion);
    }

    /// <inheritdoc />
    public async Task<ListSchemaResponse> ListSchemaAsync(ListSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await client.ListAsync(new SchemaListRequest
        {
            TenantId = "t1", PageSize = (uint)request.PageSize, ContinuousToken = request.ContinuousToken ?? string.Empty
        }).ResponseAsync;

        return new ListSchemaResponse(
            response.Head,
            response.Schemas
                .Select(item => new ListSchemaResponse.SchemaItem(item.Version, DateTime.Parse(item.CreatedAt)))
                .ToList(),
            response.ContinuousToken
        );
    }
}