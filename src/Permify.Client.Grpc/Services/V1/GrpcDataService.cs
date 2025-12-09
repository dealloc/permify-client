using Base.V1;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
using Permify.Client.Grpc.Mappers.V1;
using Permify.Client.Models.V1.Data;
using Permify.Client.Options;

using Attribute = Base.V1.Attribute;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// Implements <see cref="IDataService" /> using gRPC.
/// </summary>
public sealed class GrpcDataService(
    IOptions<PermifyOptions> options,
    Base.V1.Data.DataClient client
) : IDataService
{
    /// <inheritdoc />
    public async Task<WriteDataResponse> WriteDataAsync(WriteDataRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.WriteAsync(new DataWriteRequest
            {
                TenantId = options.Value.TenantId,
                Metadata = new DataWriteRequestMetadata
                {
                    SchemaVersion = request.Metadata?.SchemaVersion ?? string.Empty
                },
                Tuples =
                {
                    request.Tuples.Select(tuple => new Base.V1.Tuple
                    {
                        Entity = new Entity
                        {
                            Id = tuple.Entity.Id,
                            Type = tuple.Entity.Type
                        },
                        Relation = tuple.Relation,
                        Subject = new Subject
                        {
                            Id = tuple.Subject.Id,
                            Type = tuple.Subject.Type,
                        }
                    })
                },
                Attributes =
                {
                    request.Attributes.Select(attribute => new Attribute
                    {
                        Entity = new Entity
                        {
                            Id = attribute.Entity.Id,
                            Type = attribute.Entity.Type
                        },
                        Attribute_ = attribute.Attribute,
                        Value = AnyValueMapper.MapToAny(attribute.Value)
                    })
                }
            }, cancellationToken: cancellationToken).ResponseAsync;

            return DataServiceMapper.MapToWriteDataResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}