using Base.V1;

using Google.Protobuf.Collections;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
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
            // ResponseAsync is nullable by gRPC design, but only null if call fails (which throws RpcException)
            var response = (await client.WriteAsync(
                new SchemaWriteRequest { TenantId = options.Value.TenantId, Schema = request.Schema },
                cancellationToken: cancellationToken).ResponseAsync)!;

            return SchemaServiceMapper.MapWriteResponse(response);
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
            // ResponseAsync is nullable by gRPC design, but only null if call fails (which throws RpcException)
            var response = (await client.ListAsync(new SchemaListRequest
            {
                TenantId = options.Value.TenantId,
                PageSize = (uint)request.PageSize,
                ContinuousToken =
                    request.ContinuousToken ?? string.Empty // gRPC doesn't use null, so we pass an empty string.
            }).ResponseAsync)!;

            return SchemaServiceMapper.MapListResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PartialSchemaUpdateResponse> PartialUpdateSchemaAsync(PartialSchemaUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.PartialWriteAsync(new SchemaPartialWriteRequest
            {
                TenantId = options.Value.TenantId,
                Metadata = new SchemaPartialWriteRequestMetadata
                {
                    SchemaVersion = request.Metadata?.SchemaVersion ?? string.Empty
                },
                Partials =
                {
                    request.Partials.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new Partials
                        {
                            Write = { kvp.Value.Write },
                            Delete = { kvp.Value.Delete },
                            Update = { kvp.Value.Update }
                        }
                    )
                }
            }, cancellationToken: cancellationToken);

            return SchemaServiceMapper.MapPartialUpdateResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}