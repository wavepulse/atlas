// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Services;
using Atlas.Contracts.Countries;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;

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
    public async Task HandleShouldGetAllCodes()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _countryRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnTheRandomizedCountry()
    {
        RandomizedCountry country = await _handler.Handle(_query, CancellationToken.None);

        country.Cca2.Should().Be(_countries[0].Cca2);
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
        FlagSvgUri = new Uri("https://www.countryflags.io/ca/flat/64.svg"),
        MapUri = new Uri("https://www.google.com/maps/place/Canada")
    };
}
