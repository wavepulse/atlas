// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Fluxor;

namespace Atlas.Web.App.Stores.Countries;

internal static class SearchCountryReducers
{
    [ReducerMethod]
    public static SearchCountryState ReduceGetAllResult(SearchCountryState state, SearchCountryActions.GetAllResult action)
        => state with { Countries = action.Countries };
}
