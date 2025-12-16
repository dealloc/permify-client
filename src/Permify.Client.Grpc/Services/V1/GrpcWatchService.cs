using System.Runtime.CompilerServices;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers.V1;
using Permify.Client.Options;

using WatchRequest = Permify.Client.Models.V1.Watch.WatchRequest;
using WatchResponse = Permify.Client.Models.V1.Watch.WatchResponse;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// gRPC implementation of the Watch service.
/// </summary>
internal sealed class GrpcWatchService(
    IOptions<PermifyOptions> options,
    Base.V1.Watch.WatchClient client
) : IWatchService
{
    /// <inheritdoc />
    public async IAsyncEnumerable<WatchResponse> WatchAsync(
        WatchRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        AsyncServerStreamingCall<Base.V1.WatchResponse> stream;

        try
        {
            stream =
                client.Watch(
                    new Base.V1.WatchRequest
                    {
                        TenantId = options.Value.TenantId,
                        SnapToken = request.SnapToken ?? string.Empty
                    }, cancellationToken: cancellationToken);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }

        try
        {
            await foreach (var response in stream.ResponseStream.ReadAllAsync(cancellationToken))
            {
                yield return WatchServiceMapper.MapToWatchResponse(response);
            }
        }
        finally
        {
            // Ensure the stream gets disposed on exception, especially since we can't try-catch yield blocks.
            stream.Dispose();
        }
    }
}