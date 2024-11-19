// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.DevMode;

internal sealed class DevModeEffect(ISender sender)
{
    [EffectMethod]
    public async Task HandleGetCountryAsync(DevModeActions.GetCountry action, IDispatcher dispatcher)
    {
        CountryResponse country = await sender.Send(new GetCountry.Query(action.Cca2)).ConfigureAwait(false);

        dispatcher.Dispatch(new DevModeActions.GetCountryResult(country));
    }
}
