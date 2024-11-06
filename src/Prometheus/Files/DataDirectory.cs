// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json;
using Prometheus.Options;

namespace Prometheus.Files;

internal sealed class DataDirectory(IDirectory directory, PathOptions options) : IDataDirectory
{
    public string? Create()
    {
        string? rootPath = directory.GetRootPath(options.Root);

        if (string.IsNullOrEmpty(rootPath))
            return null;

        string dataPath = Path.Combine(rootPath, options.Output, DataJsonPaths.BaseDirectory);

        return directory.Create(dataPath);
    }
}
