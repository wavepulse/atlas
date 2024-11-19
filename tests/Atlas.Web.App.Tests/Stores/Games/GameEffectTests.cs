// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Fluxor;
using Mediator;
using NSubstitute.ReturnsExtensions;

namespace Atlas.Web.App.Stores.Games;

public sealed class GameEffectTests
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly ILocalStorage _storage = Substitute.For<ILocalStorage>();
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();
    private readonly ISender _sender = Substitute.For<ISender>();

    private readonly GameEffect _effect;

    public GameEffectTests()
    {
        _effect = new GameEffect(_sender, _storage, _timeService);
    }

    [Fact]
    public async Task RandomizeAsyncShouldSendRandomizeCountryQuery()
    {
        await _effect.RandomizeAsync(_dispatcher);

        await _sender.Received(1).Send(Arg.Any<RandomizeCountry.Query>(), CancellationToken.None);
    }

    [Fact]
    public async Task RandomizeAsyncShouldDispatchRandomizeResult()
    {
        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));

        _sender.Send(Arg.Any<RandomizeCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.RandomizeAsync(_dispatcher);
        _dispatcher.Received(1).Dispatch(Arg.Is<GameActions.RandomizeResult>(a => a.Country == country));
    }

    [Fact]
    public async Task GuessAsyncShouldSendGuessCountryCommand()
    {
        GameActions.Guess action = new("CA", "US");

        await _effect.GuessAsync(action, _dispatcher);

        await _sender.Received(1).Send(Arg.Is(new GuessCountry.Command(action.GuessedCca2, action.Cca2)), CancellationToken.None);
    }

    [Fact]
    public async Task GuessAsyncShouldDispatchGuessResult()
    {
        GuessedCountryResponse guessedCountry = new()
        {
            Cca2 = "CA",
            Name = "Canada",
            IsSameContinent = true,
            Kilometers = 100,
            Direction = 90,
            Success = true,
            Flag = new ImageResponse(new Uri("https://image.com"), "image/png")
        };

        _sender.Send(Arg.Any<GuessCountry.Command>(), CancellationToken.None).Returns(guessedCountry);
        GameActions.Guess action = new("CA", "US");

        await _effect.GuessAsync(action, _dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<GameActions.GuessResult>(a => a.Country == guessedCountry));
    }

    [Fact]
    public async Task DailyAsyncShouldSendGetDailyCountryQuery()
    {
        await _effect.GetDailyAsync(_dispatcher);

        await _sender.Received(1).Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None);
    }

    [Fact]
    public async Task DailyAsyncShouldDispatchDailyResult()
    {
        GuessedCountryResponse guess = CreateGuessedCountry();

        DateOnly today = new(2024, 11, 08);
        _timeService.Today.Returns(today);
        _storage.GetItem<DateOnly>(DailyStorageKeys.TodayKey).Returns(today);

        GuessedCountryResponse[] guesses = [guess];
        _storage.GetItem<GuessedCountryResponse[]>(DailyStorageKeys.GuessesKey).Returns(guesses);

        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));

        _sender.Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.GetDailyAsync(_dispatcher);
        _dispatcher.Received(1).Dispatch(Arg.Is<GameActions.GetDailyResult>(a => a.Country == country && a.Guesses.Contains(guess)));
    }

    [Fact]
    public async Task DailyAsyncShouldClearGuessesWhenIsNewDaily()
    {
        DateOnly today = new(2024, 11, 08);

        _timeService.Today.Returns(today);
        _storage.GetItem<DateOnly>(DailyStorageKeys.TodayKey).Returns(today.AddDays(-1));

        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));
        _sender.Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.GetDailyAsync(_dispatcher);

        _storage.Received(1).RemoveItem(DailyStorageKeys.GuessesKey);
    }

    [Fact]
    public async Task DailyAsyncShouldSetNewDailyDateWhenIsNewDaily()
    {
        DateOnly today = new(2024, 11, 08);
        _timeService.Today.Returns(today);

        DateOnly yesterday = today.AddDays(-1);
        _storage.GetItem<DateOnly>(DailyStorageKeys.TodayKey).Returns(yesterday);

        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));
        _sender.Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.GetDailyAsync(_dispatcher);

        _storage.Received(1).SetItem(DailyStorageKeys.TodayKey, today);
    }

    [Fact]
    public async Task DailyAsyncShouldGetGuesses()
    {
        DateOnly today = new(2024, 11, 08);
        _timeService.Today.Returns(today);
        _storage.GetItem<DateOnly>(DailyStorageKeys.TodayKey).Returns(today);

        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));
        _sender.Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.GetDailyAsync(_dispatcher);

        _storage.Received(1).GetItem<GuessedCountryResponse[]>(DailyStorageKeys.GuessesKey);
    }

    [Fact]
    public async Task DailyAsyncShouldDispatchDailyResultWithEmptyGuessesWhenDoesNotFoundInStorage()
    {
        GuessedCountryResponse guess = CreateGuessedCountry();

        DateOnly today = new(2024, 11, 08);
        _timeService.Today.Returns(today);
        _storage.GetItem<DateOnly>(DailyStorageKeys.TodayKey).Returns(today);

        _storage.GetItem<GuessedCountryResponse[]>(DailyStorageKeys.GuessesKey).ReturnsNull();

        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));

        _sender.Send(Arg.Any<GetDailyCountry.Query>(), CancellationToken.None).Returns(country);

        await _effect.GetDailyAsync(_dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<GameActions.GetDailyResult>(a => a.Guesses.Length == 0));
    }

    private static GuessedCountryResponse CreateGuessedCountry() => new()
    {
        Cca2 = "CA",
        Name = "Canada",
        IsSameContinent = true,
        Kilometers = 100,
        Direction = 90,
        Success = true,
        Flag = new ImageResponse(new Uri("https://image.com"), "image/png")
    };
}
