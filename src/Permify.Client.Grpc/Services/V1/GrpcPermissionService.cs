using Base.V1;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
using Permify.Client.Grpc.Mappers.V1;
using Permify.Client.Models.V1.Permission;
using Permify.Client.Options;

using Attribute = Base.V1.Attribute;
using Context = Base.V1.Context;
using Tuple = Base.V1.Tuple;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// Implements <see cref="IPermissionService" /> using gRPC.
/// </summary>
internal sealed class GrpcPermissionService(
    IOptions<PermifyOptions> options,
    Base.V1.Permission.PermissionClient client
) : IPermissionService
{
    /// <inheritdoc />
    public async Task<CheckAccessControlResponse> CheckAccessControlAsync(
        CheckAccessControlRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.CheckAsync(
                new PermissionCheckRequest
                {
                    TenantId = options.Value.TenantId,
                    Metadata =
                        new PermissionCheckRequestMetadata
                        {
                            SnapToken = request.Metadata.SnapToken ?? string.Empty,
                            SchemaVersion = request.Metadata.SchemaVersion ?? string.Empty,
                            Depth = request.Metadata.Depth
                        },
                    Entity = new Entity { Type = request.Entity.Type, Id = request.Entity.Id },
                    Permission = request.Permission,
                    Subject =
                        new Subject
                        {
                            Type = request.Subject.Type,
                            Id = request.Subject.Id,
                            Relation = request.Subject.Relation ?? string.Empty
                        },
                    Context = new Context
                    {
                        Tuples =
                        {
                            request.Context?.Tuples.Select(tuple => new Tuple
                            {
                                Entity = new Entity { Type = tuple.Entity.Type, Id = tuple.Entity.Id },
                                Relation = tuple.Relation,
                                Subject = new Subject { Type = tuple.Subject.Type, Id = tuple.Subject.Id, }
                            }) ?? []
                        },
                        Attributes =
                        {
                            request.Context?.Attributes.Select(attribute => new Attribute
                            {
                                Entity = new Entity { Type = attribute.Entity.Type, Id = attribute.Entity.Id },
                                Attribute_ = attribute.Attribute,
                                Value = AnyValueMapper.MapToAny(attribute.Value)
                            }) ?? []
                        },
                        Data = new Struct
                        {
                            Fields =
                            {
                                request.Context?.Data.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => new Value { StringValue = kvp.Value }
                                ) ?? []
                            }
                        }
                    }
                },
                cancellationToken: cancellationToken
            );

            return PermissionServiceMapper.MapToCheckAccessControlResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}