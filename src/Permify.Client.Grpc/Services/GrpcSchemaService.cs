using System.Globalization;

using Base.V1;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts;
using Permify.Client.Exceptions;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Models.Schema;
using Permify.Client.Options;

namespace Permify.Client.Grpc.Services;

/// <summary>
/// Implements <see cref="ISchemaService"/> using gRPC.
/// </summary>
public sealed class GrpcSchemaService(
    IOptions<PermifyOptions> options,
    Schema.SchemaClient client
) : ISchemaService
{
    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.WriteAsync(
                new SchemaWriteRequest { TenantId = options.Value.TenantId, Schema = request.Schema },
                cancellationToken: cancellationToken).ResponseAsync;

            return new WriteSchemaResponse(response.SchemaVersion);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ListSchemaResponse> ListSchemaAsync(ListSchemaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.ListAsync(new SchemaListRequest
            {
                TenantId = options.Value.TenantId,
                PageSize = (uint)request.PageSize,
                ContinuousToken =
                    request.ContinuousToken ?? string.Empty // gRPC doesn't use null, so we pass an empty string.
            }).ResponseAsync;

            return new ListSchemaResponse(
                response.Head,
                response.Schemas
                    .Select(item => new ListSchemaResponse.SchemaItem(item.Version, DateTimeOffset.ParseExact(
                        item.CreatedAt,
                        "yyyy-MM-dd HH:mm:ss zzz 'UTC'",
                        CultureInfo.InvariantCulture
                    )))
                    .ToList(),
                response.ContinuousToken
            );
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}