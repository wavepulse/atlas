// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace Atlas.Infrastructure.Json.Caching;

public sealed class AppCacheTests
{
    private readonly CacheSettings _settings = new()
    {
        ExpirationTimeInMinutes = 3600
    };

    private readonly IMemoryCache _cache = Substitute.For<IMemoryCache>();

    private readonly AppCache _appCache;

    public AppCacheTests()
    {
        _appCache = new AppCache(_cache, _settings);
    }

    [Fact]
    public async Task GetOrCreateAsyncShouldCreateTheEntryWhenDoesNotFoundTheEntry()
    {
        await _appCache.GetOrAddAsync("key", () => Task.FromResult("value"));

        _cache.Received(1).CreateEntry("key");
    }

    [Fact]
    public async Task GetOrCreateAsyncShouldSetSettingsForTheCacheEntry()
    {
        ICacheEntry entry = Substitute.For<ICacheEntry>();
        _cache.CreateEntry("key").ReturnsForAnyArgs(entry);

        await _appCache.GetOrAddAsync("key", () => Task.FromResult("value"));

        entry.AbsoluteExpirationRelativeToNow.Should().Be(TimeSpan.FromMinutes(_settings.ExpirationTimeInMinutes));
    }

    [Fact]
    public async Task GetOrCreateAsyncShouldReturnTheValue()
    {
        ICacheEntry entry = Substitute.For<ICacheEntry>();
        _cache.CreateEntry("key").ReturnsForAnyArgs(entry);

        string value = await _appCache.GetOrAddAsync("key", () => Task.FromResult("value"));

        value.Should().Be("value");
    }
}
