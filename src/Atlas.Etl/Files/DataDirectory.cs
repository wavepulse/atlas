// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Settings;
using Atlas.Infrastructure.Json;

namespace Atlas.Etl.Files;

internal sealed class DataDirectory(IDirectory directory, PathSettings settings) : IDataDirectory
{
    public string? Create()
    {
        string? rootPath = directory.GetRootPath(settings.Root);

        if (string.IsNullOrEmpty(rootPath))
            return null;

        string dataPath = Path.Combine(rootPath, settings.Output, DataJsonPaths.BaseDirectory);

        return directory.Create(dataPath);
    }
}
