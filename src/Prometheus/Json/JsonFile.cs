// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Json;

[ExcludeFromCodeCoverage]
internal sealed class JsonFile : IJsonFile
{
    public Stream OpenWrite(string path) => File.OpenWrite(path);
}
