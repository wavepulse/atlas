// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Caching;
using Atlas.Infrastructure.Changelog.Sources;

namespace Atlas.Infrastructure.Changelog.Repositories;

public sealed class ChangelogRepositoryTests
{
    private const string ExpectedKey = "changelog";

    private readonly IChangelogClient _client = Substitute.For<IChangelogClient>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly ChangelogRepository _repository;

    public ChangelogRepositoryTests()
    {
        _repository = new ChangelogRepository(_client, _cache);
    }

    [Fact]
    public async Task GetAsyncShouldGetTheChangelog()
    {
        await _repository.GetAsync(CancellationToken.None);

        await _client.Received(1).GetAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetAsyncShouldCacheTheChangelog()
    {
        const string changelog = "test!";
        _client.GetAsync(CancellationToken.None).Returns(changelog);

        await _repository.GetAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedKey, changelog);
    }

    [Fact]
    public async Task GetAsyncShouldNotRetrieveFromClientIsChangelogCached()
    {
        _cache.TryGet<string>(ExpectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(CancellationToken.None);

        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }
}
