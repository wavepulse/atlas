// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Games.Components;

public sealed class GameOverTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();

    public GameOverTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddLocalization();
    }

    [Fact]
    public void ComponentShouldRenderCongratutionSectionWhenCountryIsNull()
    {
        IRenderedComponent<GameOver> component = RenderComponent<GameOver>(parameters =>
            parameters.Add(g => g.MapUri, new Uri("https://example.com")));

        IElement? element = component.Nodes.QuerySelector("div.congratulations");

        element.Should().NotBeNull();
    }

    [Fact]
    public void ComponentShouldRenderGameOverSectionWhenCountryIsNotNull()
    {
        IRenderedComponent<GameOver> component = RenderComponent<GameOver>(parameters =>
            parameters.Add(g => g.Country, "Canada")
                      .Add(g => g.MapUri, new Uri("https://example.com")));

        IElement? element = component.Nodes.QuerySelector("div.game-over > a");

        element.Should().NotBeNull();
    }

    [Fact]
    public void RestartButtonShouldDispatchRestartAction()
    {
        IRenderedComponent<GameOver> component = RenderComponent<GameOver>(parameters =>
            parameters.Add(g => g.MapUri, new Uri("https://example.com"))
                      .Add(g => g.OnRestart, () => { }));

        IElement element = component.Find("button");
        element.Click();

        _dispatcher.Received().Dispatch(Arg.Any<GameActions.Restart>());
    }

    [Fact]
    public void RestartButtonShouldInvokeEventCallback()
    {
        bool isCalled = false;

        IRenderedComponent<GameOver> component = RenderComponent<GameOver>(parameters =>
            parameters.Add(g => g.MapUri, new Uri("https://example.com"))
                      .Add(g => g.OnRestart, () => isCalled = true));

        IElement element = component.Find("button");
        element.Click();

        isCalled.Should().BeTrue();
    }

    [Fact]
    public void ComponentShouldNotRenderRestartButtonWhenOnRestartIsNull()
    {
        IRenderedComponent<GameOver> component = RenderComponent<GameOver>(parameters =>
            parameters.Add(g => g.MapUri, new Uri("https://example.com")));

        IElement? element = component.Nodes.QuerySelector("button");

        element.Should().BeNull();
    }
}
