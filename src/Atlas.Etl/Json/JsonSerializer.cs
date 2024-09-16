// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using Serializer = System.Text.Json.JsonSerializer;

namespace Atlas.Etl.Json;

[ExcludeFromCodeCoverage]
internal sealed class JsonSerializer : IJsonSerializer
{
    public Task SerializeAsync<T>(Stream stream, T value, JsonTypeInfo<T> metadata, CancellationToken cancellationToken)
        => Serializer.SerializeAsync(stream, value, metadata, cancellationToken);
}
