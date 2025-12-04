using System.Text.Json.Serialization;

using Permify.Client.Models.Schema;

namespace Permify.Client;

/// <summary>
/// Contains source generated JSON type info for AOT compatible JSON serialization.
/// </summary>
[JsonSerializable(typeof(WriteSchemaRequest))]
[JsonSerializable(typeof(WriteSchemaResponse))]
public sealed partial class PermifyJsonSerializerContext : JsonSerializerContext;