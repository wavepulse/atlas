// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Application.Flags.Commands;
using Atlas.Contracts.Flags;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Countries;

internal sealed class CountryEffects(ISender sender)
{
    [EffectMethod(typeof(CountryActions.Randomize))]
    public async Task RandomizeAsync(IDispatcher dispatcher)
    {
        string randomizedCca2 = await sender.Send(new RandomizeCountry.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.RandomizeResult(randomizedCca2));
    }

    [EffectMethod]
    public async Task GuessAsync(CountryActions.Guess action, IDispatcher dispatcher)
    {
        GuessedFlag guessedFlag = await sender.Send(new GuessFlag.Command(action.GuessedCca2, action.RandomizedCca2)).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.GuessResult(guessedFlag));
    }
}
