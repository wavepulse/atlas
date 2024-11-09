// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Games.Components;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Games;
using Bunit.TestDoubles;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Games.Flags;

public sealed class DailyFlagTests : TestContext
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
    private readonly ILocalStorage _localStorage = Substitute.For<ILocalStorage>();
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();
    private readonly ISender _sender = Substitute.For<ISender>();

    public DailyFlagTests()
    {
        ComponentFactories.AddStub<CountryLookupInput>();
        ComponentFactories.AddStub<GameOver>();

        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_localStorage);
        Services.AddSingleton(_timeService);
        Services.AddSingleton(_sender);

        _sender.Send(Arg.Any<GetDailyCountry.Query>()).Returns(_country);
        _sender.Send(Arg.Any<GuessCountry.Command>()).Returns(_guessedCountry);
    }

    [Fact]
    public void PageShouldDispatchGetDailyAction()
    {
        RenderComponent<DailyFlag>();

        _dispatcher.Received().Dispatch(Arg.Any<GameActions.GetDaily>());
    }

    [Fact]
    public void PageShouldSubscribeToGetDailyResultAction()
    {
        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.GetDailyResult>>());
    }

    [Fact]
    public void PageShouldSubscribeToGuessResultAction()
    {
        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.GuessResult>>());
    }

    [Fact]
    public void PageShouldDisposeSubscriber()
    {
        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        page.Instance.Dispose();

        _subscriber.Received().UnsubscribeFromAllActions(page.Instance);
    }

    [Fact]
    public void PageShouldRenderCountryLookupInputWhenGameIsNotFinished()
    {
        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        page.HasComponent<Stub<CountryLookupInput>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldRenderGameOverWhenGameIsFinished()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.GetDailyResult(_country, []));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry));
        });

        page.HasComponent<Stub<GameOver>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldRenderGameOverFromDailyResultWhenContainsAnSuccessFromGuesses()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GetDailyResult(_country, [_guessedCountry])));

        page.HasComponent<Stub<GameOver>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldRenderGameOverFromDailyResultWhenContainsMaxAttemptsAndNoSuccess()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        GuessedCountryResponse[] guesses = Enumerable.Range(0, 6).Select(_ => _guessedCountry with { Success = false }).ToArray();

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GetDailyResult(_country, guesses)));

        page.HasComponent<Stub<GameOver>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldAddGuessedCountriesWhenFailedToGuessTheCountry()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();
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

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.GetDailyResult(_country, []));
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

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        await page.InvokeAsync(() =>
        {
            dispatcher.Dispatch(new GameActions.GetDailyResult(_country, []));

            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
            dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry with { Success = false }));
        });

        page.HasComponent<Stub<GameOver>>().Should().BeTrue();
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

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();
        IRenderedComponent<Stub<CountryLookupInput>> input = page.FindComponent<Stub<CountryLookupInput>>();

        EventCallback<string> eventCallback = input.Instance.Parameters.Get(i => i.Guess);

        await page.InvokeAsync(() => eventCallback.InvokeAsync("US"));

        guessAction.Should().NotBeNull();
    }

    [Fact]
    public async Task PageShouldAddGuessedCountryToLocalStorage()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<DailyFlag> page = RenderComponent<DailyFlag>();

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.GuessResult(_guessedCountry)));

        _localStorage.Received().SetItem(DailyStorageKeys.GuessesKey, Arg.Is<List<GuessedCountryResponse>>(c => c.Contains(_guessedCountry)));
    }
}
