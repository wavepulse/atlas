// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Fluxor;

namespace Atlas.Web.App.Stores.Countries;

internal static class CountryReducers
{
    [ReducerMethod]
    public static CountryState ReduceLookupResult(CountryState state, CountryActions.LookupResult action)
        => state with { Countries = action.Countries };
}
