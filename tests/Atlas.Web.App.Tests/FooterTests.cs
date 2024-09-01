// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Components;
using Atlas.Web.App.Services;
using Atlas.Web.App.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App;

public sealed class FooterTests : BunitContext
{
    private readonly ProjectSettings _projectSettings = new()
    {
        Version = "1.0.0",
        Url = "https://test.com"
    };

    private readonly CompanySettings _companySettings = new()
    {
        Name = "Pulsewave",
        Url = "https://pulsewave.test.com"
    };

    private readonly ITime _time = Substitute.For<ITime>();

    public FooterTests()
    {
        Services.AddSingleton(_projectSettings);
        Services.AddSingleton(_companySettings);
        Services.AddSingleton(_time);

        _time.UtcNow.Returns(new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public void FooterShouldDisplayOneCopyrightYearWhenCurrentYearIsEqualToStartYear()
    {
        IRenderedComponent<Footer> footer = Render<Footer>();

        IElement li = footer.Find("li:first-child");

        li.TextContent.Should().Be("© 2024");
    }

    [Fact]
    public void FooterShouldDisplayRangeOfCopyrightYearWhenCurrentYearIsNotEqualToStartYear()
    {
        _time.UtcNow.Returns(new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc));

        IRenderedComponent<Footer> footer = Render<Footer>();

        IElement li = footer.Find("li:first-child");

        li.TextContent.Should().Be("© 2024 - 2025");
    }

    [Fact]
    public void FooterShoulRenderOnce()
    {
        IRenderedComponent<Footer> footer = Render<Footer>();
        footer.Render();

        footer.RenderCount.Should().Be(1);
    }
}
