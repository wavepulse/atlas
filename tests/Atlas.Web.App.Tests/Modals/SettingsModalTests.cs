// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Services;
using Atlas.Web.App.Settings;
using Atlas.Web.App.Settings.Components;
using Atlas.Web.App.Settings.Modals;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Settings;
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
        IStateSelection<SettingsState, General> state = Substitute.For<IStateSelection<SettingsState, General>>();
        state.Value.Returns(new General() { Theme = Theme.Dark });

        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(_sender);
        Services.AddSingleton(Substitute.For<ILocalStorage>());
        Services.AddSingleton(Substitute.For<ITimeService>());
        Services.AddSingleton(state);
        Services.AddLocalization();

        JSInterop.SetupVoid("showModal", _ => true).SetVoidResult();
        JSInterop.SetupVoid("scrollContentToTop", _ => true).SetVoidResult();
        JSInterop.SetupVoid("closeModal", _ => true).SetVoidResult();
        JSInterop.SetupVoid("addCloseOutsideEvent", _ => true).SetVoidResult();
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
    public void ShowGeneralShouldInvokeJavascript()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.ShowGeneral();

        JSInterop.VerifyInvoke("showModal");
        JSInterop.VerifyInvoke("scrollContentToTop");
    }

    [Fact]
    public void ShowChangelogShouldInvokeJavascript()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();
        modal.Instance.ShowChangelog();

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
    public void ModalShouldAddActiveOnSelectedTab()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        IElement item = modal.Find("li:not(.active)");

        item.Click();

        IElement? activeItem = modal.Nodes.QuerySelector("li.active");

        activeItem.Should().NotBeNull();
    }

    [Fact]
    public void ModalShouldGetGeneralCssForContentWhenSelectGeneral()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.ShowGeneral();
        modal.Render();

        IElement content = modal.Find(".content");

        content.ClassList.Should().Contain("general");
    }

    [Fact]
    public void ModalShouldHaveGeneralComponentWhenSelectedTabIsGeneral()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.ShowGeneral();
        modal.Render();

        modal.HasComponent<GeneralSettings>().Should().BeTrue();
    }

    [Fact]
    public void ModalShouldGetChangelogCssForContentWhenSelectChangelog()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.ShowChangelog();
        modal.Render();

        IElement content = modal.Find(".content");

        content.ClassList.Should().Contain("changelog");
    }

    [Fact]
    public void ModalShouldHaveChangelogComponentWhenSelectedTabIsChangelog()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();

        modal.Instance.ShowChangelog();
        modal.Render();

        modal.HasComponent<Changelog>().Should().BeTrue();
    }
}
