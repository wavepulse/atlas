// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Games.Components;
using Atlas.Web.App.Options;
using Atlas.Web.App.Services;
using Atlas.Web.App.Settings;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.DevMode;
using Atlas.Web.App.Stores.Games;
using Bunit.TestDoubles;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Games.Flags;

public sealed class RandomizedFlagTests : Bunit.TestContext
{
    private readonly AppSettings _settings = new();
    private readonly DevModeOptions _devMode = new();
    private readonly CountryResponse _country = new("CA", "Canada", new ImageResponse(new Uri("https://example.com"), "image/png"), new Uri("https://map.com"));
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

    public RandomizedFlagTests()
    {
        ComponentFactories.AddStub<CountryLookupInput>();
        ComponentFactories.AddStub<GameOver>();

        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_localStorage);
        Services.AddSingleton(_timeService);
        Services.AddSingleton(_sender);
        Services.AddSingleton(_devMode);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddLocalization();

        _sender.Send(Arg.Any<RandomizeCountry.Query>()).Returns(_country);
        _sender.Send(Arg.Any<GuessCountry.Command>()).Returns(_guessedCountry);
    }

    [Fact]
    public void PageShouldDispatchRandomizeAction()
    {
        RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _dispatcher.Received().Dispatch(Arg.Any<GameActions.Randomize>());
    }

    [Fact]
    public void PageShouldDispatchGetCountryActionWhenDevModeIsEnabledAndHavingCca2()
    {
        const string cca2 = "CA";
        _devMode.Enabled = true;

        NavigationManager navigation = Services.GetRequiredService<NavigationManager>();
        string uri = navigation.GetUriWithQueryParameter("cca2", cca2);

        navigation.NavigateTo(uri);

        RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _dispatcher.Received().Dispatch(Arg.Is<DevModeActions.GetCountry>(c => c.Cca2 == cca2));
    }

    [Theory]
    [InlineData(true, "")]
    [InlineData(false, "CA")]
    public void PageShouldNotDispatchGetCountryActionWhenDoesNotMeetConditionToUseDevMode(bool isEnabled, string cca2)
    {
        _devMode.Enabled = isEnabled;

        NavigationManager navigation = Services.GetRequiredService<NavigationManager>();
        string uri = navigation.GetUriWithQueryParameter("cca2", cca2);

        navigation.NavigateTo(uri);

        RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _dispatcher.DidNotReceive().Dispatch(Arg.Is<DevModeActions.GetCountry>(c => c.Cca2 == cca2));
    }

    [Fact]
    public void PageShouldSubscribeToGetCountryResultActionWhenDevModeIsEnabled()
    {
        _devMode.Enabled = true;

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<DevModeActions.GetCountryResult>>());
    }

    [Fact]
    public void PageShouldNotSubscribeToGetCountryResultActionWhenDevModeIsNotEnabled()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _subscriber.DidNotReceive().SubscribeToAction(page.Instance, Arg.Any<Action<DevModeActions.GetCountryResult>>());
    }

    [Fact]
    public void PageShouldSubscribeToRandomizeResultAction()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.RandomizeResult>>());
    }

    [Fact]
    public void PageShouldSubscribeToGuessResultAction()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        _subscriber.Received().SubscribeToAction(page.Instance, Arg.Any<Action<GameActions.GuessResult>>());
    }

    [Fact]
    public void PageShouldDisposeSubscriber()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        page.Instance.Dispose();

        _subscriber.Received().UnsubscribeFromAllActions(page.Instance);
    }

    [Fact]
    public void PageShouldRenderCountryLookupInputWhenGameIsNotFinished()
    {
        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

        page.HasComponent<Stub<CountryLookupInput>>().Should().BeTrue();
    }

    [Fact]
    public async Task PageShouldRenderGameOverWhenGameIsFinished()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));
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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));
        IRenderedComponent<Stub<CountryLookupInput>> input = page.FindComponent<Stub<CountryLookupInput>>();

        EventCallback<string> eventCallback = input.Instance.Parameters.Get(i => i.Guess);

        await page.InvokeAsync(() => eventCallback.InvokeAsync("US"));

        guessAction.Should().NotBeNull();
    }

    [Fact]
    public async Task RestartShouldDispatchRandomizeAction()
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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

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

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(_settings));

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

    [Theory]
    [InlineData(Difficulty.Blur, "blur-0")]
    [InlineData(Difficulty.Invert, "invert")]
    [InlineData(Difficulty.Shift, "shift")]
    public async Task PageShouldAddDifficultyWhenAppSettingsContainsADifficultyInAll(Difficulty difficulty, string css)
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        AppSettings settings = new() { Flag = new FlagDifficulty() { All = difficulty } };

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(settings));

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.RandomizeResult(_country)));

        IElement element = page.Find("picture");

        element.ClassList.Should().Contain(css);
    }

    [Theory]
    [InlineData(Difficulty.Blur, "blur-0")]
    [InlineData(Difficulty.Invert, "invert")]
    [InlineData(Difficulty.Shift, "shift")]
    public async Task PageShouldAddDifficultyWhenAppSettingsContainsADifficultyInRandomized(Difficulty difficulty, string css)
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        AppSettings settings = new() { Flag = new FlagDifficulty() { All = Difficulty.None, Randomized = difficulty } };

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(settings));

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.RandomizeResult(_country)));

        IElement element = page.Find("picture");

        element.ClassList.Should().Contain(css);
    }

    [Fact]
    public async Task PageShouldAddNoDifficultyWhenAppSettingsContainsNoneForAll()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        AppSettings settings = new() { Flag = new FlagDifficulty() { All = Difficulty.None } };

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(settings));

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.RandomizeResult(_country)));

        IElement element = page.Find("picture");

        element.ClassList.Should().Contain("country-image");
    }

    [Fact]
    public async Task PageShouldAddNoDifficultyWhenAppSettingsContainsNoneForRandomized()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        AppSettings settings = new() { Flag = new FlagDifficulty() { Randomized = Difficulty.None } };

        IRenderedComponent<RandomizedFlag> page = RenderComponent<RandomizedFlag>(parameters => parameters.AddCascadingValue(settings));

        await page.InvokeAsync(() => dispatcher.Dispatch(new GameActions.RandomizeResult(_country)));

        IElement element = page.Find("picture");

        element.ClassList.Should().Contain("country-image");
    }
}
