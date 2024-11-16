// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Modals;
using Atlas.Web.App.Options;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Components;

public sealed class HeaderTests : TestContext
{
    public HeaderTests()
    {
        ProjectOptions project = new()
        {
            BugUrl = "https://bug.com",
            Url = "https://atlas.com",
            Version = "1.0.0"
        };

        Services.AddSingleton(project);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(Substitute.For<IDispatcher>());
        Services.AddSingleton(Substitute.For<IActionSubscriber>());
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
