// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace Atlas.Infrastructure.Json.Caching;

internal sealed class AppCache(IMemoryCache cache, CacheSettings settings) : IAppCache
{
    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory)
    {
        MemoryCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(settings.ExpirationTimeInMinutes)
        };

        T? entry = await cache.GetOrCreateAsync(key, _ => factory(), options).ConfigureAwait(false);

        return entry!;
    }
}
