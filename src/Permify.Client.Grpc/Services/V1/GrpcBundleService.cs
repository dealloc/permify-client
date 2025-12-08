using Base.V1;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Models.V1.Bundles;
using Permify.Client.Options;

using BundleServiceMapper = Permify.Client.Grpc.Mappers.V1.BundleServiceMapper;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// Implements <see cref="IBundleService"/> using gRPC.
/// </summary>
public sealed class GrpcBundleService(
    IOptions<PermifyOptions> options,
    Base.V1.Bundle.BundleClient client
) : IBundleService
{
    /// <inheritdoc />
    public async Task<WriteBundleResponse> WriteBundleAsync(
        WriteBundleRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // ResponseAsync is nullable by gRPC design, but only null if call fails (which throws RpcException)
            var response = (await client.WriteAsync(
                new BundleWriteRequest
                {
                    TenantId = options.Value.TenantId,
                    Bundles =
                    {
                        request.Bundles.Select(bundle => new DataBundle
                        {
                            Name = bundle.Name,
                            Arguments = { bundle.Arguments },
                            Operations =
                            {
                                bundle.Operations.Select(operation => new Base.V1.Operation
                                {
                                    AttributesWrite = { operation.AttributesWrite },
                                    AttributesDelete = { operation.AttributesDelete },
                                    RelationshipsWrite = { operation.RelationshipsWrite },
                                    RelationshipsDelete = { operation.RelationshipsDelete }
                                })
                            }
                        })
                    }
                },
                cancellationToken: cancellationToken).ResponseAsync)!;

            return BundleServiceMapper.MapToWriteBundleResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ReadBundleResponse> ReadBundleAsync(
        ReadBundleRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // ResponseAsync is nullable by gRPC design, but only null if call fails (which throws RpcException)
            var response = await client.ReadAsync(
                new BundleReadRequest
                {
                    TenantId = options.Value.TenantId,
                    Name = request.Name
                },
                cancellationToken: cancellationToken
            ).ResponseAsync;

            return BundleServiceMapper.MapToReadBundleResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}