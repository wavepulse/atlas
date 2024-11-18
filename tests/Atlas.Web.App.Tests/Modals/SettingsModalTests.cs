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
using Microsoft.JSInterop;

namespace Atlas.Web.App.Modals;

public sealed class SettingsModalTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly ISender _sender = Substitute.For<ISender>();

    public SettingsModalTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(_sender);
        Services.AddSingleton(Substitute.For<ILocalStorage>());
        Services.AddSingleton(Substitute.For<ITimeService>());
        Services.AddLocalization();

        JSInterop.SetupVoid("showModal", _ => true).SetVoidResult();
        JSInterop.SetupVoid("scrollContentToTop", _ => true).SetVoidResult();
        JSInterop.SetupVoid("closeModal", _ => true).SetVoidResult();
        JSInterop.SetupVoid("addCloseOutsideEvent", _ => true).SetVoidResult();
    }

    [Fact]
    public void ModalShouldDispatchToGetChangelog()
    {
        RenderComponent<SettingsModal>();

        _dispatcher.Received(1).Dispatch(Arg.Any<ChangelogActions.GetChangelog>());
    }

    [Fact]
    public void ModalShouldSubscribeToGetChangelogResult()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        _subscriber.Received(1).SubscribeToAction(modal.Instance, Arg.Any<Action<ChangelogActions.GetChangelogResult>>());
    }

    [Fact]
    public void PageShouldDisposeSubscriber()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.Dispose();

        _subscriber.Received().UnsubscribeFromAllActions(modal.Instance);
    }

    [Fact]
    public void ModalShouldInvokeClickOutsideEvent()
    {
        RenderComponent<SettingsModal>();

        JSInterop.VerifyInvoke("addCloseOutsideEvent");
    }

    [Fact]
    public void ShowShouldInvokeJavascript()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.Show();

        JSInterop.VerifyInvoke("showModal");
        JSInterop.VerifyInvoke("scrollContentToTop");
    }

    [Fact]
    public void ModalShouldCloseTheModal()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        IElement close = modal.Find(".close");

        close.Click();

        JSInterop.VerifyInvoke("closeModal");
    }

    [Fact]
    public async Task ModalShouldPopulateChangelogContent()
    {
        const string changelog = "changelog";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(changelog);

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        await modal.InvokeAsync(() => dispatcher.Dispatch(new ChangelogActions.GetChangelog()));

        IElement content = modal.Find(".content");

        content.TextContent.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ChangelogContentShouldHaveAttributesForHeader()
    {
        const string changelog = "## header";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(changelog);

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        IElement content = modal.Find(".content > :first-child");

        content.MarkupMatches("<h2 class=\"version\">header</h2>\n");
    }

    [Fact]
    public async Task ChangelogContentShouldHaveAttributesForLinks()
    {
        const string changelog = "test ([#34](https://bunit.dev))";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(changelog);

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        IElement content = modal.Find(".content > :first-child");

        content.MarkupMatches("<p>test (<a href=\"https://bunit.dev\" class=\"link\" target=\"_blank\">#34</a>)</p>");
    }

    [Fact]
    public async Task ChangelogContentShouldHaveAttributesForList()
    {
        const string changelog = "- test";

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        _sender.Send(Arg.Any<GetChangelog.Query>(), CancellationToken.None).Returns(changelog);

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        IElement content = modal.Find(".content > :first-child");

        content.MarkupMatches("<ul class=\"section\"><li>test</li></ul>");
    }
}
