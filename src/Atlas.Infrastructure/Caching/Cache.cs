// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Caching.Options;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Caching;

[ExcludeFromCodeCoverage]
internal sealed class Cache(IMemoryCache memoryCache, CacheOptions options) : ICache
{
    public void Save<TItem>(string key, TItem value)
        => memoryCache.Set(key, value, TimeSpan.FromMinutes(options.ExpirationTimeInMinutes));

    public bool TryGet<TItem>(string key, [NotNullWhen(true)] out TItem? value)
        => memoryCache.TryGetValue(key, out value);
}
