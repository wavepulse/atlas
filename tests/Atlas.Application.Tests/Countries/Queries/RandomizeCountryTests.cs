// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Services;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries.Queries;

public sealed class RandomizeCountryTests
{
    private readonly Country[] _countries = [CreateCanada()];

    private readonly IRandomizer _randomizer = new Randomizer();
    private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();

    private readonly RandomizeCountry.Query _query = new();
    private readonly RandomizeCountry.Handler _handler;

    public RandomizeCountryTests()
    {
        _countryRepository.GetAllAsync(CancellationToken.None).Returns(_countries);

        _handler = new RandomizeCountry.Handler(_randomizer, _countryRepository);
    }

    [Fact]
    public async Task HandleShouldGetAllCountries()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _countryRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnTheRandomizedCountry()
    {
        CountryResponse country = await _handler.Handle(_query, CancellationToken.None);

        country.Cca2.Should().Be(_countries[0].Cca2);
    }

    [Fact]
    public async Task HandleShouldCacheTheRandomizedCountry()
    {
        await _handler.Handle(_query, CancellationToken.None);

        _countryRepository.Received(1).Cache(Arg.Is<Country>(c => c.Cca2 == _countries[0].Cca2));
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = ["USA"],
        Capitals = [new Capital("Ottawa", new Coordinate(42, 42))],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, 95),
        Population = 38008005,
        Translations = [new Translation(Language.English, "Canada")],
        IsExcluded = false,
        Resources = new CountryResources()
        {
            Flag = new Image(new Uri("https://www.countryflags.io/ca/flat/64.png"), "image/png"),
            Map = new Uri("https://www.google.com/maps/place/Canada")
        }
    };
}
