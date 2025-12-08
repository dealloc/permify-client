using Base.V1;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Models.V1.Schema;
using Permify.Client.Options;

using SchemaServiceMapper = Permify.Client.Grpc.Mappers.V1.SchemaServiceMapper;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// Implements <see cref="ISchemaService"/> using gRPC.
/// </summary>
public sealed class GrpcSchemaService(
    IOptions<PermifyOptions> options,
    Schema.SchemaClient client
) : ISchemaService
{
    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(
        WriteSchemaRequest request,
        CancellationToken cancellationToken
    )
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
    public async Task<ListSchemaResponse> ListSchemaAsync(
        ListSchemaRequest request,
        CancellationToken cancellationToken
    )
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
            }, cancellationToken: cancellationToken).ResponseAsync)!;

            return SchemaServiceMapper.MapListResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PartialSchemaUpdateResponse> PartialUpdateSchemaAsync(
        PartialSchemaUpdateRequest request,
        CancellationToken cancellationToken
    )
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