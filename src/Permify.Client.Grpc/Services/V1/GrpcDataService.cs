using Base.V1;

using Grpc.Core;

using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
using Permify.Client.Grpc.Mappers.V1;
using Permify.Client.Models.V1.Data;
using Permify.Client.Options;

using Attribute = Base.V1.Attribute;
using AttributeFilter = Base.V1.AttributeFilter;
using EntityFilter = Base.V1.EntityFilter;
using SubjectFilter = Base.V1.SubjectFilter;
using TupleFilter = Base.V1.TupleFilter;

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

    /// <inheritdoc />
    public async Task<ReadRelationshipsResponse> ReadRelationshipsAsync(
        ReadRelationshipsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.ReadRelationshipsAsync(new RelationshipReadRequest
            {
                TenantId = options.Value.TenantId,
                Metadata = new RelationshipReadRequestMetadata
                {
                    SnapToken = request.Metadata.SnapToken ?? string.Empty,
                },
                Filter = new TupleFilter
                {
                    Entity = new EntityFilter
                    {
                        Type = request.Filter.Entity?.Type ?? string.Empty,
                        Ids = { request.Filter.Entity?.Ids ?? [] }
                    },
                    Relation = request.Filter.Relation ?? string.Empty,
                    Subject = new SubjectFilter
                    {
                        Type = request.Filter.Subject?.Type ?? string.Empty,
                        Ids = { request.Filter.Subject?.Ids ?? [] },
                        Relation = request.Filter.Subject?.Relation ?? string.Empty
                    }
                },
                PageSize = request.PageSize,
                ContinuousToken = request.ContinuousToken ?? string.Empty
            }, cancellationToken: cancellationToken).ResponseAsync;

            return DataServiceMapper.MapToReadRelationshipsResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ReadAttributesResponse> ReadAttributesAsync(
        ReadAttributesRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.ReadAttributesAsync(new AttributeReadRequest
            {
                TenantId = options.Value.TenantId,
                Metadata = new AttributeReadRequestMetadata
                {
                    SnapToken = request.Metadata?.SnapToken ?? string.Empty,
                },
                Filter = new AttributeFilter
                {
                    Entity = new EntityFilter
                    {
                        Type = request.Filter?.Entity?.Type ?? string.Empty,
                        Ids = { request.Filter?.Entity?.Ids ?? [] }
                    },
                    Attributes = { request.Filter?.Attributes ?? [] }
                },
                PageSize = request.PageSize,
                ContinuousToken = request.ContinuousToken ?? string.Empty
            }, cancellationToken: cancellationToken).ResponseAsync;

            return DataServiceMapper.MapToReadAttributesResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RunBundleResponse> RunBundleAsync(
        RunBundleRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.RunBundleAsync(new BundleRunRequest
            {
                TenantId = options.Value.TenantId,
                Name = request.Name,
                Arguments =
                {
                    request.Attributes
                }
            }, cancellationToken: cancellationToken).ResponseAsync;

            return DataServiceMapper.MapToRunBundleResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<DeleteDataResponse> DeleteDataAsync(
        DeleteDataRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.DeleteAsync(new DataDeleteRequest
            {
                TenantId = options.Value.TenantId,
                TupleFilter = new TupleFilter
                {
                    Entity = new EntityFilter
                    {
                        Type = request.TupleFilter.Entity?.Type ?? string.Empty,
                        Ids = { request.TupleFilter.Entity?.Ids ?? [] }
                    },
                    Relation = request.TupleFilter.Relation ?? string.Empty,
                    Subject = new SubjectFilter
                    {
                        Type = request.TupleFilter.Subject?.Type ?? string.Empty,
                        Ids = { request.TupleFilter.Subject?.Ids ?? [] },
                        Relation = request.TupleFilter.Subject?.Relation ?? string.Empty
                    }
                },
                AttributeFilter = new AttributeFilter
                {
                    Entity = new EntityFilter
                    {
                        Type = request.TupleFilter.Entity?.Type ?? string.Empty,
                        Ids = { request.TupleFilter.Entity?.Ids ?? [] }
                    },
                    Attributes = { request.AttributeFilter.Attributes ?? [] }
                }
            }, cancellationToken: cancellationToken).ResponseAsync;

            return DataServiceMapper.MapToDeleteDataResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}