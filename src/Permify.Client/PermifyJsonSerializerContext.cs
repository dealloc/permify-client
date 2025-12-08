using System.Text.Json.Serialization;

using Permify.Client.Models.V1.Schema;

namespace Permify.Client;

/// <summary>
/// Contains source generated JSON type info for AOT compatible JSON serialization.
/// </summary>
[JsonSerializable(typeof(WriteSchemaRequest))]
[JsonSerializable(typeof(WriteSchemaResponse))]
[JsonSerializable(typeof(ListSchemaRequest))]
[JsonSerializable(typeof(ListSchemaResponse))]
public sealed partial class PermifyJsonSerializerContext : JsonSerializerContext;