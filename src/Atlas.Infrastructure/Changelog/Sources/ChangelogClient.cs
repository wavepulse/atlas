// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Changelog.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Changelog.Sources;

[ExcludeFromCodeCoverage]
internal sealed class ChangelogClient(HttpClient client, ChangelogOptions options) : IChangelogClient
{
    public async Task<string?> GetAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await client.GetAsync(options.Url, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }
}
