using Permify.Client.Http.Generated.Models;
using Permify.Client.Mappers;
using Permify.Client.Models.V1.Bundles;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Http.Mappers.V1;

/// <summary>
/// Maps HTTP Bundle responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class BundleServiceMapper
{
    /// <summary>
    /// Maps a Kiota WriteBundleResponse to domain BundleWriteResponse.
    /// </summary>
    public static partial WriteBundleResponse MapWriteBundleResponse(BundleWriteResponse response);

    /// <summary>
    /// Maps a Kiota ReadBundleResponse to domain BundleReadResponse.
    /// </summary>
    public static partial ReadBundleResponse MapReadBundleResponse(BundleReadResponse response);
}