// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Options;
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

        JSInterop.SetupVoid("toggleNavigation").SetVoidResult();
    }

    [Fact]
    public void HeaderShouldToggleMenuWhenClickingOnTheButton()
    {
        IRenderedComponent<Header> header = RenderComponent<Header>();

        IElement button = header.Find("button");

        button.Click();

        JSInterop.VerifyInvoke("toggleNavigation");
    }
}
