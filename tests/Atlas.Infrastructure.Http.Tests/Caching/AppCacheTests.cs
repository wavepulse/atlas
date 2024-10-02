// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Http.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace Atlas.Infrastructure.Http.Caching;

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
    public void CreateEntryShouldCreateAnEntry()
    {
        using ICacheEntry entry = _appCache.CreateEntry("key");

        _cache.Received(1).CreateEntry("key");
    }

    [Fact]
    public void CreateEntryShouldCreateAnEntryWithSettings()
    {
        using ICacheEntry entry = Substitute.For<ICacheEntry>();

        _cache.CreateEntry("key").Returns(entry);

        _ = _appCache.CreateEntry("key");

        entry.AbsoluteExpirationRelativeToNow.Should().Be(TimeSpan.FromMinutes(_settings.ExpirationTimeInMinutes));
    }

    [Fact]
    public void TryGetValueShouldReturnTheBoolean()
    {
        _cache.TryGetValue("key", out _).Returns(returnThis: true);

        bool result = _appCache.TryGetValue<object>("key", out _);

        result.Should().BeTrue();
    }

    [Fact]
    public void TryGetValueShouldOutputTheValue()
    {
        _cache.TryGetValue("key", out _).Returns(x =>
        {
            x[1] = "value";
            return true;
        });

        bool result = _appCache.TryGetValue("key", out string? value);

        value.Should().Be("value");
    }
}
