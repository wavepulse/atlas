// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Infrastructure.Changelog.Sources;

internal interface IChangelogClient
{
    Task<string> GetAsync(CancellationToken cancellationToken);
}
