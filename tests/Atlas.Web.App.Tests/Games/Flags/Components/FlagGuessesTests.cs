// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Components;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace Atlas.Web.App.Games.Flags.Components;

public sealed class FlagGuessesTests : TestContext
{
    private readonly GuessedCountryResponse _country = new()
    {
        Cca2 = "GB",
        Name = "United Kingdom",
        IsSameContinent = true,
        Kilometers = 1000,
        Direction = 45,
        Success = true,
        Flag = new ImageResponse(new Uri("https://flag.svg"), MediaTypeNames.Image.Svg)
    };

    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();

    public FlagGuessesTests()
    {
        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_dispatcher);
    }

    [Fact]
    public void ComponentShouldRenderSixNonGuessedFlagsWhenThereIsNoGuessedFlags()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");

        nonGuessedFlags.Should().HaveCount(6);
    }

    [Fact]
    public void ComponentShouldRenderNonGuessedFlagsWithGoodIndex()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");

        nonGuessedFlags[0].TextContent.Should().Be("1 / 6");
    }

    [Fact]
    public void ComponentShouldSubscribeToGuessResultAction()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        _subscriber.Received().SubscribeToAction(component.Instance, Arg.Any<Action<GameActions.GuessResult>>());
    }

    [Fact]
    public void ComponentShouldSubscribeToRestartAction()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        _subscriber.Received().SubscribeToAction(component.Instance, Arg.Any<Action<GameActions.Restart>>());
    }

    [Fact]
    public void ComponentShouldDisposeActionSubscriber()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        component.Instance.Dispose();

        _subscriber.Received().UnsubscribeFromAllActions(component.Instance);
    }

    [Fact]
    public async Task ComponentShouldPopulateGuessedFlagsWhenGuessResultActionIsDispatched()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));

        IRefreshableElementCollection<IElement> guessedFlags = component.FindAll("li.guess:not(.empty)");
        guessedFlags.Should().ContainSingle();

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");
        nonGuessedFlags.Should().HaveCount(5);
    }

    [Fact]
    public async Task ComponentShouldDispatchGameOverActionWhenMaxGuessesAreReachedAndCountryWasNotGuessed()
    {
        bool isDispatched = false;

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        dispatcher.ActionDispatched += (sender, args) =>
        {
            if (args.Action is GameActions.GameOver)
            {
                isDispatched = true;
            }
        };

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() =>
        {
            for (int i = 0; i < 6; i++)
            {
                dispatcher.Dispatch(new GameActions.GuessResult(_country with { Success = false }));
            }
        });

        isDispatched.Should().BeTrue();
    }

    [Fact]
    public async Task ComponentShouldClearGuessedFlagsWhenRestartActionIsDispatched()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));
        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.Restart()));

        IRefreshableElementCollection<IElement> guessedFlags = component.FindAll("li.guess:not(.empty)");
        guessedFlags.Should().BeEmpty();

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");
        nonGuessedFlags.Should().HaveCount(6);
    }

    [Fact]
    public async Task GuessedCountryShouldHaveSuccessCssWhenIsGoodCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));

        IElement guessedFlag = component.Find("li.guess:first-child");

        guessedFlag.ClassName.Should().Contain("success");
    }

    [Fact]
    public async Task GuessedCountryShouldHaveWrongCssWhenIsBadCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country with { Success = false })));

        IElement guessedFlag = component.Find("li.guess:first-child");

        guessedFlag.ClassName.Should().Contain("wrong");
    }

    [Fact]
    public async Task GuessedCountryShouldHaveCheckCssForArrowWhenIsGoodCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));

        IElement arrow = component.Find("li.guess i.arrow");

        arrow.ClassName.Should().Contain($"success {Icons.Check}");
    }

    [Fact]
    public async Task GuessedCountryShouldHaveDefaultCssForArrowWhenIsBadCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country with { Success = false })));

        IElement arrow = component.Find("li.guess i.arrow");

        arrow.ClassName.Should().Contain(Icons.ArrowUp);
    }

    [Fact]
    public async Task GuessedCountryShouldHaveCheckCssForContinentWhenIsSameContinent()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));

        IElement sameContinent = component.Find("li.guess i.same-continent");

        sameContinent.ClassName.Should().Contain($"success {Icons.Check}");
    }

    [Fact]
    public async Task GuessedCountryShouldHaveTimesCssForContinentWhenIsNotSameContinent()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country with { IsSameContinent = false })));

        IElement sameContinent = component.Find("li.guess i.same-continent");

        sameContinent.ClassName.Should().Contain($"wrong {Icons.Times}");
    }

    [Fact]
    public async Task NonGuessedCountryShouldHaveNoCssWhenIsNotTheGoodCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country with { Success = false })));

        IElement nonGuessedFlag = component.Find("li.guess.empty");

        nonGuessedFlag.ClassName.Should().NotContain("has-won");
    }

    [Fact]
    public async Task NonGuessedCountryShouldHaveHasWonCssWhenIsGoodCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>();

        await component.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_country)));

        IElement nonGuessedFlag = component.Find("li.guess.empty");

        nonGuessedFlag.ClassName.Should().Contain("has-won");
    }
}
