// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Etl.Files;

[ExcludeFromCodeCoverage]
internal sealed class Directory : IDirectory
{
    public string Create(string path) => System.IO.Directory.CreateDirectory(path).FullName;

    public string? GetRootPath(string root)
    {
        DirectoryInfo current = new(System.IO.Directory.GetCurrentDirectory());

        while (current.Parent is not null)
        {
            if (string.Equals(current.Name, root, StringComparison.OrdinalIgnoreCase))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return null;
    }
}
