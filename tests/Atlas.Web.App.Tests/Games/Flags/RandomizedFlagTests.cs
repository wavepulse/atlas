// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Games.Components;
using Atlas.Web.App.Stores.Games;
using Bunit.TestDoubles;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Games.Flags;

public sealed class RandomizedFlagTests : TestContext
{
    private readonly RandomizedCountryResponse _country = new("CA", "Canada", new ImageResponse(new Uri("https://example.com"), "image/png"), new Uri("https://map.com"));
    private readonly GuessedCountryResponse _guessedCountry = new()
    {
        Cca2 = "CA",
        Name = "Canada",
        IsSameContinent = true,
        Kilometers = 0,
        Direction = 0,
        Success = true,
        Flag = new ImageResponse(new Uri("https://example.com"), "image/png")
    };

    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly ISender _sender = Substitute.For<ISender>();

    public RandomizedFlagTests()
    {
        ComponentFactories.AddStub<CountryLookupInput>();
        ComponentFactories.AddStub<GameOver>();

        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_sender);

        _sender.Send(Arg.Any<RandomizeCountry.Query>()).Returns(_country);
        _sender.Send(Arg.Any<GuessCountry.Command>()).Returns(_guessedCountry);
    }

    [Fact]
    public void PageShouldDispatchRandomizeAction()
    {
        RenderComponent<RandomizedFlag>();

        _dispatcher.Received().Dispatch(Arg.Any<GameActions.Randomize>());
    }

    [Fact]
    public void PageShouldSubscribeToRandomizeResultAction()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.RandomizeResult>>());
    }

    [Fact]
    public void PageShouldSubscribeToGuessResultAction()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.GuessResult>>());
    }

    [Fact]
    public void PageShouldDisposeSubscriber()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        page.Instance.Dispose();

        _subscriber.Received().UnsubscribeFromAllActions(page.Instance);
    }

    [Fact]
    public void PageShouldRenderCountryLookupInputWhenGameIsNotFinished()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        page.HasComponent<Stub<CountryLookupInput>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldRenderGameOverWhenGameIsFinished()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.RandomizeResult(_country));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry));
        });

        page.HasComponent<Stub<GameOver>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldAddGuessedCountriesWhenFailedToGuessTheCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();
        IRenderedComponent<Stub<CountryLookupInput>> input = page.FindComponent<Stub<CountryLookupInput>>();

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false })));

        IEnumerable<string> guessedCountries = input.Instance.Parameters.Get(i => i.SelectedCountries);

        guessedCountries.Should().Contain("CA");
    }

    [Fact]
    public async Task PageShouldFinishTheGameWhenFoundTheCountryToGuess()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.RandomizeResult(_country));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry));
        });

        IRenderedComponent<Stub<GameOver>> gameOver = page.FindComponent<Stub<GameOver>>();

        string? country = gameOver.Instance.Parameters.Get(i => i.Country);

        country.Should().BeNull();
    }

    [Fact]
    public async Task PageShouldFinishTheGameWhenHavingMaxAttempts()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.RandomizeResult(_country));

            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
        });

        bool result = page.HasComponent<Stub<GameOver>>();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GuessShouldDispatchGuessAction()
    {
        GameActions.Guess? guessAction = null;

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        dispatcher.ActionDispatched += (sender, args) =>
        {
            if (args.Action is GameActions.Guess guess)
                guessAction = guess;
        };

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();
        IRenderedComponent<Stub<CountryLookupInput>> input = page.FindComponent<Stub<CountryLookupInput>>();

        EventCallback<string> eventCallback = input.Instance.Parameters.Get(i => i.Guess);

        await page.InvokeAsync(() => eventCallback.InvokeAsync("US"));

        guessAction.Should().NotBeNull();
    }

    [Fact]
    public async Task RestartShouldDispatchRamomizeAction()
    {
        GameActions.Randomize? action = null;

        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        dispatcher.ActionDispatched += (sender, args) =>
        {
            if (args.Action is GameActions.Randomize randomizeAction)
                action = randomizeAction;
        };

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.RandomizeResult(_country));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry));
        });

        IRenderedComponent<Stub<GameOver>> gameOver = page.FindComponent<Stub<GameOver>>();

        EventCallback eventCallback = gameOver.Instance.Parameters.Get(i => i.OnRestart);

        await page.InvokeAsync(eventCallback.InvokeAsync);

        action.Should().NotBeNull();
    }

    [Fact]
    public async Task RestartShouldClearGuessedCountries()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.RandomizeResult(_country));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry));
        });

        IRenderedComponent<Stub<GameOver>> gameOver = page.FindComponent<Stub<GameOver>>();

        EventCallback eventCallback = gameOver.Instance.Parameters.Get(i => i.OnRestart);

        await page.InvokeAsync(eventCallback.InvokeAsync);

        IRenderedComponent<Stub<CountryLookupInput>> input = page.FindComponent<Stub<CountryLookupInput>>();

        IEnumerable<string> guessedCountries = input.Instance.Parameters.Get(i => i.SelectedCountries);

        guessedCountries.Should().BeEmpty();
    }
}
