// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Countries;

public sealed class CountryEffectTests
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly ISender _sender = Substitute.For<ISender>();

    private readonly CountryEffect _effect;

    public CountryEffectTests()
    {
        _effect = new CountryEffect(_sender);
    }

    [Fact]
    public async Task LookupAsyncShouldSendLookupCountriesQuery()
    {
        await _effect.LookupAsync(_dispatcher);

        await _sender.Received(1).Send(Arg.Any<LookupCountries.Query>());
    }

    [Fact]
    public async Task LookupAsyncShouldDispatchLookupResult()
    {
        CountryLookupResponse country = new("CA", "Canada");

        CountryLookupResponse[] countries = [country];

        _sender.Send(Arg.Any<LookupCountries.Query>()).Returns(countries);

        await _effect.LookupAsync(_dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<CountryActions.LookupResult>(a => a.Countries.Contains(country)));
    }
}
