// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Changelog.Queries;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Changelog;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Settings.Components;

public sealed class ChangelogTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly ILocalStorage _localStorage = Substitute.For<ILocalStorage>();
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();
    private readonly ISender _sender = Substitute.For<ISender>();

    public ChangelogTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_sender);
        Services.AddSingleton(_localStorage);
        Services.AddSingleton(_timeService);
    }

    [Fact]
    public void ChangelogShouldDispatchGetChangelog()
    {
        RenderComponent<Changelog>();

        _dispatcher.Received(1).Dispatch(Arg.Any<ChangelogActions.GetChangelog>());
    }

    [Fact]
    public void ChangelogShouldSubscribeToGetChangelogResult()
    {
        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        _subscriber.Received(1).SubscribeToAction(changelog.Instance, Arg.Any<Action<ChangelogActions.GetChangelogResult>>());
    }

    [Fact]
    public void ChangelogShouldDisposeSubscriber()
    {
        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        changelog.Instance.Dispose();

        _subscriber.Received(1).UnsubscribeFromAllActions(changelog.Instance);
    }

    [Fact]
    public void ChangelogShouldDisplaySpinnerWhenContentIsNotReady()
    {
        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        IElement? spinner = changelog.Nodes.QuerySelector("span.spinner");

        spinner.Should().NotBeNull();
    }

    [Fact]
    public async Task ChangelogShouldPopulateContentWhenIsReady()
    {
        const string content = "changelog";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(content);

        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        await changelog.InvokeAsync(() => dispatcher.Dispatch(new ChangelogActions.GetChangelog()));

        changelog.MarkupMatches("<p>changelog</p>");
    }

    [Fact]
    public async Task ChangelogShouldAddAttributesForHeader()
    {
        const string content = "## header";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(content);

        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        changelog.MarkupMatches("<h2 class=\"version\">header</h2>\n");
    }

    [Fact]
    public async Task ChangelogShouldAddAttributesForLinks()
    {
        const string content = "test ([#34](https://bunit.dev))";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(content);

        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        changelog.MarkupMatches("<p>test (<a href=\"https://bunit.dev\" class=\"link\" target=\"_blank\">#34</a>)</p>");
    }

    [Fact]
    public async Task ChangelogShouldAddAttributesForList()
    {
        const string content = "- test";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(content);

        IRenderedComponent<Changelog> changelog = RenderComponent<Changelog>();

        changelog.MarkupMatches("<ul class=\"section\"><li>test</li></ul>");
    }
}
