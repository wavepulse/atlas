// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Changelog;

public sealed class ChangelogEffectTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();

    private readonly ChangelogEffect _effect;

    public ChangelogEffectTests()
    {
        _effect = new ChangelogEffect(_sender);
    }

    [Fact]
    public async Task GetChangelogAsyncShouldSendGetChangelogQuery()
    {
        await _effect.GetChangelogAsync(_dispatcher);

        await _sender.Received(1).Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None);
    }

    [Fact]
    public async Task GetChangelogAsyncShouldDispatchTheResult()
    {
        const string changelog = "changelog";

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(changelog);

        await _effect.GetChangelogAsync(_dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<ChangelogActions.GetChangelogResult>(c => c.Changelog == changelog));
    }
}
