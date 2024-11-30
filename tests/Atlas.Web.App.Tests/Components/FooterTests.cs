// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Options;
using Atlas.Web.App.Services;
using Atlas.Web.App.Settings.Modals;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Components;

public sealed class FooterTests : Bunit.TestContext
{
    private readonly DateOnly _today = new(2024, 11, 06);

    private readonly ITimeService _timeService = Substitute.For<ITimeService>();

    public FooterTests()
    {
        ProjectOptions project = new()
        {
            BugUrl = "https://bug.com",
            Url = "https://atlas.com",
            Version = "1.0.0"
        };

        CompanyOptions company = new()
        {
            Name = "Pulsewave",
            Url = "https://pulsewave.com"
        };

        Services.AddTransient(_ => _timeService);
        Services.AddSingleton(project);
        Services.AddSingleton(company);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(Substitute.For<IDispatcher>());
        Services.AddSingleton(Substitute.For<IActionSubscriber>());
        Services.AddLocalization();

        JSInterop.SetupVoid("addCloseOutsideEvent", _ => true).SetVoidResult();
    }

    [Fact]
    public void FooterShouldRenderFooterOnce()
    {
        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();
        IRenderedComponent<Footer> footer = RenderComponent<Footer>(parameters => parameters.AddCascadingValue(modal.Instance));

        footer.Render();

        footer.RenderCount.Should().Be(1);
    }

    [Fact]
    public void FooterShouldDisplayCurrentCopyrightYearWhenStartYearIsEqualToCurrentYear()
    {
        int startYear = _today.Year;
        _timeService.Today.Returns(_today);

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();
        IRenderedComponent<Footer> footer = RenderComponent<Footer>(parameters => parameters.AddCascadingValue(modal.Instance));

        IElement element = footer.Find("ul > li:first-child");

        element.TextContent.Should().Be($"© {startYear}");
    }

    [Fact]
    public void FooterShouldDisplayRangeOfYearsWhenStartYearIsNotEqualToCurrentYear()
    {
        int startYear = _today.Year;
        int endYear = _today.AddYears(1).Year;

        _timeService.Today.Returns(_today.AddYears(1));

        IRenderedComponent<SettingsModal> modal = RenderComponent<SettingsModal>();
        IRenderedComponent<Footer> footer = RenderComponent<Footer>(parameters => parameters.AddCascadingValue(modal.Instance));

        IElement element = footer.Find("ul > li:first-child");

        element.TextContent.Should().Be($"© {startYear} - {endYear}");
    }
}
