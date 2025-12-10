using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Generated.V1.Tenants.Item.Permissions;
using Permify.Client.Http.Mappers;
using Permify.Client.Http.Mappers.V1;
using Permify.Client.Models.V1.Permission;
using Permify.Client.Options;

using Context = Permify.Client.Http.Generated.Models.Context;

namespace Permify.Client.Http.Services.V1;

internal sealed class HttpPermissionService(
    IOptions<PermifyOptions> options,
    ApiClient api
) : IPermissionService
{
    /// <summary>Shorthand utility for the <see cref="PermissionsRequestBuilder" />.</summary>
    private PermissionsRequestBuilder Permissions => api.V1.Tenants[options.Value.TenantId].Permissions;

    /// <inheritdoc />
    public async Task<CheckAccessControlResponse> CheckAccessControlAsync(
        CheckAccessControlRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await Permissions.Check.PostAsync(
                new CheckBody
                {
                    Metadata = new PermissionCheckRequestMetadata
                    {
                        SnapToken = request.Metadata.SnapToken ?? string.Empty,
                        SchemaVersion = request.Metadata.SchemaVersion ?? string.Empty,
                        Depth = request.Metadata.Depth
                    },
                    Entity = new Entity
                    {
                        Type = request.Entity.Type,
                        Id = request.Entity.Id
                    },
                    Permission = request.Permission,
                    Subject = new Subject
                    {
                        Type = request.Subject.Type,
                        Id = request.Subject.Id,
                        Relation = request.Subject.Relation
                    },
                    Context = new Context
                    {
                        Tuples = request.Context?.Tuples.Select(tuple => new TupleObject
                        {
                            Entity = new Entity { Type = tuple.Entity.Type, Id = tuple.Entity.Id },
                            Relation = tuple.Relation,
                            Subject = new Subject { Type = tuple.Subject.Type, Id = tuple.Subject.Id, }
                        }).ToList() ?? [],
                        Attributes = request.Context?.Attributes.Select(attribute => new AttributeObject
                        {
                            Entity = new Entity { Type = attribute.Entity.Type, Id = attribute.Entity.Id },
                            Attribute = attribute.Attribute,
                            Value = AnyValueMapper.MapToAny(attribute.Value)
                        }).ToList() ?? [],
                        Data = new Context_data
                        {
                            AdditionalData = request.Context?.Data.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value as object
                            ) ?? []
                        }
                    }
                }, cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return PermissionServiceMapper.MapToCheckAccessControlResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}