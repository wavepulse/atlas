// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries.Queries;

public sealed class GetCountryTests
{
    private readonly Country _country = CreateCanada();

    private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();

    private readonly GetCountry.Query _query;
    private readonly GetCountry.Handler _handler;

    public GetCountryTests()
    {
        _countryRepository.GetAsync(_country.Cca2, CancellationToken.None).Returns(_country);

        _query = new GetCountry.Query(_country.Cca2);
        _handler = new GetCountry.Handler(_countryRepository);
    }

    [Fact]
    public async Task HandleShouldGetTheCountry()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _countryRepository.Received(1).GetAsync(_country.Cca2, CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnTheCountry()
    {
        CountryResponse country = await _handler.Handle(_query, CancellationToken.None);

        country.Cca2.Should().Be(_country.Cca2);
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
