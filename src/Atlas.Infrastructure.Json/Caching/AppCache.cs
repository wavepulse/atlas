// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Json.Caching;

internal sealed class AppCache(IMemoryCache cache, CacheSettings settings) : IAppCache
{
    public ICacheEntry CreateEntry(string key)
    {
        ICacheEntry entry = cache.CreateEntry(key);

        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(settings.ExpirationTimeInMinutes);

        return entry;
    }

    public bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? value)
        => cache.TryGetValue(key, out value);
}
