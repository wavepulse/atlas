// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Options;
using Atlas.Web.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Components;

public sealed class FooterTests : TestContext
{
    private readonly DateTimeOffset _now = new(new DateTime(2024, 01, 01, 01, 01, 01, DateTimeKind.Utc));

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
        Services.AddSingleton(_ => project);
        Services.AddSingleton(_ => company);
    }

    [Fact]
    public void ShouldRenderFooterOnce()
    {
        IRenderedComponent<Footer> footer = RenderComponent<Footer>();
        footer.Render();

        footer.RenderCount.Should().Be(1);
    }

    [Fact]
    public void ShouldDisplayCurrentCopyrightYearWhenStartYearIsEqualToCurrentYear()
    {
        int startYear = _now.Year;
        _timeService.UtcNow.Returns(_now);

        IRenderedComponent<Footer> footer = RenderComponent<Footer>();

        IElement element = footer.Find("ul > li:first-child");

        element.TextContent.Should().Be($"© {startYear}");
    }

    [Fact]
    public void ShouldDisplayRangeOfYearsWhenStartYearIsNotEqualToCurrentYear()
    {
        int startYear = _now.Year;
        int endYear = _now.AddYears(1).Year;

        _timeService.UtcNow.Returns(_now.AddYears(1));

        IRenderedComponent<Footer> footer = RenderComponent<Footer>();

        IElement element = footer.Find("ul > li:first-child");

        element.TextContent.Should().Be($"© {startYear} - {endYear}");
    }
}
