// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Countries;

internal sealed class CountryEffect(ISender sender)
{
    [EffectMethod(typeof(CountryActions.Randomize))]
    public async Task RandomizeAsync(IDispatcher dispatcher)
    {
        RandomizedCountryResponse country = await sender.Send(new RandomizeCountry.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.RandomizeResult(country));
    }

    [EffectMethod]
    public async Task GuessAsync(CountryActions.Guess action, IDispatcher dispatcher)
    {
        GuessedCountryResponse guessedFlag = await sender.Send(new GuessCountry.Command(action.GuessedCca2, action.RandomizedCca2)).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.GuessResult(guessedFlag));
    }

    [EffectMethod(typeof(CountryActions.Lookup))]
    public async Task LookupAsync(IDispatcher dispatcher)
    {
        CountryLookupResponse[] countries = await sender.Send(new LookupCountries.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.LookupResult(countries));
    }
}
