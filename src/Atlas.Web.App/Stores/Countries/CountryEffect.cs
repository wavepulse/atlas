// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Countries;

internal sealed class CountryEffect(ISender sender)
{
    [EffectMethod(typeof(CountryActions.Lookup))]
    public async Task LookupAsync(IDispatcher dispatcher)
    {
        CountryLookupResponse[] countries = await sender.Send(new LookupCountries.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new CountryActions.LookupResult(countries));
    }
}
