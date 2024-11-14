// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;

namespace Atlas.Application.Changelog.Queries;

public sealed class GetChangelogTests
{
    private readonly IChangelogRepository _changelogRepository = Substitute.For<IChangelogRepository>();

    private readonly GetChangelog.Query _query = new();
    private readonly GetChangelog.Handler _handler;

    public GetChangelogTests()
    {
        _handler = new GetChangelog.Handler(_changelogRepository);
    }

    [Fact]
    public async Task HandleShouldGetChangelog()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _changelogRepository.Received(1).GetAsync(CancellationToken.None);
    }
}
