// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Options;
using Atlas.Web.App.Settings;
using Atlas.Web.App.Settings.Modals;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Components;

public sealed class HeaderTests : Bunit.TestContext
{
    public HeaderTests()
    {
        ProjectOptions project = new()
        {
            BugUrl = "https://bug.com",
            Url = "https://atlas.com",
            Version = "1.0.0"
        };

        IStateSelection<SettingsState, General> state = Substitute.For<IStateSelection<SettingsState, General>>();
        state.Value.Returns(new General() { Theme = Theme.Dark });

        Services.AddSingleton(project);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(Substitute.For<IDispatcher>());
        Services.AddSingleton(Substitute.For<IActionSubscriber>());
        Services.AddSingleton(state);
        Services.AddLocalization();

        JSInterop.SetupVoid("toggleNavigation").SetVoidResult();
        JSInterop.SetupVoid("addCloseOutsideEvent", _ => true).SetVoidResult();
    }

    [Fact]
    public void HeaderShouldToggleMenuWhenClickingOnTheButton()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();
        IRenderedComponent<Header> header = RenderComponent<Header>(parameters => parameters.AddCascadingValue(modal.Instance));

        IElement button = header.Find("button.hamburger");

        button.Click();

        JSInterop.VerifyInvoke("toggleNavigation");
    }
}
