// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus.Files;

internal interface IDirectory
{
    string Create(string path);

    string? GetRootPath(string root);
}
