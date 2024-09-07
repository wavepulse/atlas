// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Contracts.Countries;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Countries;

internal sealed class SearchCountryEffects(ISender sender)
{
    [EffectMethod(typeof(SearchCountryActions.GetAll))]
    public async Task GetAllAsync(IDispatcher dispatcher)
    {
        SearchCountry[] countries = await sender.Send(new GetAllSearchCountries.Query()).ConfigureAwait(false);
        dispatcher.Dispatch(new SearchCountryActions.GetAllResult(countries));
    }
}
