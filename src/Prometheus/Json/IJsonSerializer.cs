// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Prometheus.Json;

internal interface IJsonSerializer
{
    Task SerializeAsync<T>(Stream stream, T value, JsonTypeInfo<T> metadata, CancellationToken cancellationToken);
}
