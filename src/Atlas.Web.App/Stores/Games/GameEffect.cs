// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Games;

internal sealed class GameEffect(ISender sender, ILocalStorage storage, ITimeService timeService)
{
    [EffectMethod(typeof(GameActions.Randomize))]
    public async Task RandomizeAsync(IDispatcher dispatcher)
    {
        CountryResponse country = await sender.Send(new RandomizeCountry.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new GameActions.RandomizeResult(country));
    }

    [EffectMethod]
    public async Task GuessAsync(GameActions.Guess action, IDispatcher dispatcher)
    {
        GuessedCountryResponse guessedFlag = await sender.Send(new GuessCountry.Command(action.GuessedCca2, action.Cca2)).ConfigureAwait(false);

        dispatcher.Dispatch(new GameActions.GuessResult(guessedFlag));
    }

    [EffectMethod(typeof(GameActions.GetDaily))]
    public async Task GetDailyAsync(IDispatcher dispatcher)
    {
        CountryResponse country = await sender.Send(new GetDailyCountry.Query()).ConfigureAwait(false);

        DateOnly today = timeService.Today;
        DateOnly lastPlayed = storage.GetItem<DateOnly>(LocalStorageKeys.Today);

        if (lastPlayed != today)
        {
            storage.RemoveItem(LocalStorageKeys.Guesses);
            storage.SetItem(LocalStorageKeys.Today, today);
        }

        GuessedCountryResponse[] guesses = storage.GetItem<GuessedCountryResponse[]>(LocalStorageKeys.Guesses) ?? [];

        dispatcher.Dispatch(new GameActions.GetDailyResult(country, guesses));
    }
}
