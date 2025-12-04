using Microsoft.Extensions.Options;

namespace Permify.Client.Options.Validators;

/// <summary>
/// Validates the <see cref="PermifyOptions"/> configuration.
/// </summary>
public sealed class ValidatePermifyOptions : IValidateOptions<PermifyOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, PermifyOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.TenantId))
            return ValidateOptionsResult.Fail("Tenant ID is required.");
        if (options.EnableServiceDiscovery && string.IsNullOrWhiteSpace(options.GrpcEndpointName))
            return ValidateOptionsResult.Fail("Service discovery is enabled but no endpoint name was provided for gRPC.");
        if (options.EnableServiceDiscovery && string.IsNullOrWhiteSpace(options.HttpEndpointName))
            return ValidateOptionsResult.Fail("Service discovery is enabled but no endpoint name was provided for HTTP.");

        return ValidateOptionsResult.Success;
    }
}