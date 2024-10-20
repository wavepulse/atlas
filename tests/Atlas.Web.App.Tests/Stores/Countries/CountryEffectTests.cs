// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
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
    public async Task RandomizeAsyncShouldSendRandomizeCountryQuery()
    {
        await _effect.RandomizeAsync(_dispatcher);

        await _sender.Received(1).Send(Arg.Any<RandomizeCountry.Query>());
    }

    [Fact]
    public async Task RandomizeAsyncShouldDispatchRandomizeResult()
    {
        RandomizedCountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));

        _sender.Send(Arg.Any<RandomizeCountry.Query>()).Returns(country);

        await _effect.RandomizeAsync(_dispatcher);
        _dispatcher.Received(1).Dispatch(Arg.Is<CountryActions.RandomizeResult>(a => a.Country == country));
    }

    [Fact]
    public async Task GuessAsyncShouldSendGuessCountryCommand()
    {
        CountryActions.Guess action = new("CA", "US");

        await _effect.GuessAsync(action, _dispatcher);

        await _sender.Received(1).Send(Arg.Is(new GuessCountry.Command(action.GuessedCca2, action.RandomizedCca2)));
    }

    [Fact]
    public async Task GuessAsyncShouldDispatchGuessResult()
    {
        GuessedCountryResponse guessedCountry = new()
        {
            Cca2 = "CA",
            Name = "Canada",
            IsSameContinent = true,
            Kilometers = 100,
            Direction = 90,
            Success = true,
            Flag = new ImageResponse(new Uri("https://image.com"), "image/png")
        };

        _sender.Send(Arg.Any<GuessCountry.Command>()).Returns(guessedCountry);
        CountryActions.Guess action = new("CA", "US");

        await _effect.GuessAsync(action, _dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<CountryActions.GuessResult>(a => a.Country == guessedCountry));
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
