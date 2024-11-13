// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Changelog.Repositories;

public interface IChangelogRepository
{
    ValueTask<string> GetAsync(CancellationToken cancellationToken);
}
