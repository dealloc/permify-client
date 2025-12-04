namespace Permify.Client.Options;

/// <summary>
/// The options to configure how the Permify client should behave.
/// </summary>
public sealed class PermifyOptions
{
    /// <summary>
    /// Which tenant ID to use, defaults to <c>"t1"</c>.
    /// </summary>
    /// <remarks>
    /// The default tenant ID is documented <a href="https://docs.permify.co/getting-started/quickstart#configuring-schema-via-api">
    /// here</a>.
    /// </remarks>
    public string TenantId { get; set; } = "t1";

    /// <summary>
    /// Enables service discovery for the underlying clients.
    /// </summary>
    public bool EnableServiceDiscovery { get; set; } = false;

    /// <summary>
    /// Used for Aspire integration, this allows specifying the name of the endpoint for gRPC when using service discovery.
    /// </summary>
    public string GrpcEndpointName { get; set; } = "grpc";

    /// <summary>
    /// Used for Aspire integration, this allows specifying the name of the endpoint for HTTP when using service discovery.
    /// </summary>
    public string HttpEndpointName { get; set; } = "http";
}