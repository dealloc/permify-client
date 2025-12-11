using System.Text.Json.Serialization;

using Permify.Client.Models.V1.Schema;
using Permify.Client.Models.V1.Watch;

namespace Permify.Client;

/// <summary>
/// Contains source generated JSON type info for AOT compatible JSON serialization.
/// </summary>
[JsonSerializable(typeof(WriteSchemaRequest))]
[JsonSerializable(typeof(WriteSchemaResponse))]
[JsonSerializable(typeof(ListSchemaRequest))]
[JsonSerializable(typeof(ListSchemaResponse))]

[JsonSerializable(typeof(WatchRequest))]
[JsonSerializable(typeof(WatchResponse))]
public sealed partial class PermifyJsonSerializerContext : JsonSerializerContext;