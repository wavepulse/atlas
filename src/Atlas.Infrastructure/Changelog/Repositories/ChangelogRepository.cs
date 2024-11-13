// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Atlas.Infrastructure.Caching;
using Atlas.Infrastructure.Changelog.Sources;

namespace Atlas.Infrastructure.Changelog.Repositories;

internal sealed class ChangelogRepository(IChangelogClient client, ICache cache) : IChangelogRepository
{
    private const string Key = "changelog";

    public async ValueTask<string> GetAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out string? cachedChangelog))
            return cachedChangelog;

        string changelog = await client.GetAsync(cancellationToken).ConfigureAwait(false);

        cache.Save(Key, changelog);

        return changelog;
    }
}
