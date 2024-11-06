// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Web.App.Stores.Countries;

public sealed class CountryReducersTests
{
    [Fact]
    public void ReduceLookupResultShouldPopulateState()
    {
        CountryState state = new();
        CountryLookupResponse[] countries = [new CountryLookupResponse("CA", "Canada")];

        CountryActions.LookupResult action = new(countries);

        CountryState updatedState = CountryReducers.ReduceLookupResult(state, action);

        updatedState.Countries.Should().BeEquivalentTo(countries);
    }
}
