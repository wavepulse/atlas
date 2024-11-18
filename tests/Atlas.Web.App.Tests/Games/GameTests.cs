// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Modals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Net.Mime;

namespace Atlas.Web.App.Games;

public sealed class GameTests : TestContext
{
    public GameTests()
    {
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddLocalization();

        JSInterop.SetupVoid("showModal", _ => true).SetVoidResult();
    }

    [Fact]
    public void GameShouldDisplayPictureWhenImageIsNotNull()
    {
        ImageResponse image = new(new Uri("https://image.svg"), MediaTypeNames.Image.Svg);

        IRenderedComponent<Game> game = RenderComponent<Game>(parameters =>
            parameters.Add(g => g.Image, image)
                      .AddChildContent("<div>content</div>"));

        IElement element = game.Find("picture.country-image");

        element.Should().NotBeNull();
    }

    [Fact]
    public void GameShouldHaveZoomModalWhenImageIsNotNull()
    {
        ImageResponse image = new(new Uri("https://image.svg"), MediaTypeNames.Image.Svg);

        IRenderedComponent<Game> game = RenderComponent<Game>(parameters =>
            parameters.Add(g => g.Image, image)
                      .AddChildContent("<div>content</div>"));

        bool result = game.HasComponent<ZoomModal>();

        result.Should().BeTrue();
    }

    [Fact]
    public void GameShouldNotDisplayPictureWhenImageIsNull()
    {
        IRenderedComponent<Game> game = RenderComponent<Game>(parameters =>
            parameters.Add(g => g.Image, value: null)
                      .AddChildContent("<div>content</div>"));

        IRefreshableElementCollection<IElement> elements = game.FindAll("picture.country-image");

        elements.Should().BeEmpty();
    }

    [Fact]
    public void GameShouldDisplayContentInsideOfADiv()
    {
        IRenderedComponent<Game> game = RenderComponent<Game>(parameters => parameters.AddChildContent("<div>content</div>"));

        IElement element = game.Find("div.content > :first-child");

        element.TextContent.Should().Be("content");
    }

    [Fact]
    public void ClickOnPictureShouldShowZoomModal()
    {
        ImageResponse image = new(new Uri("https://image.svg"), MediaTypeNames.Image.Svg);

        IRenderedComponent<Game> game = RenderComponent<Game>(parameters =>
            parameters.Add(g => g.Image, image)
                      .AddChildContent("<div>content</div>"));

        IElement picture = game.Find("picture");

        picture.Click();

        JSInterop.VerifyInvoke("showModal");
    }
}
