using Base.V1;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
using Permify.Client.Models.Bundles;
using Permify.Client.Options;

namespace Permify.Client.Grpc.Services;

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
}