// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;

namespace Atlas.Etl.Countries.Mappers;

public sealed class SearchCountryMapperTests
{
    private readonly Country _canada = CreateCanada();

    [Fact]
    public void AsSearchCountryShouldMapDomainToSearchCountry()
    {
        Country[] countries = [_canada];

        SearchCountry[] searchCountries = countries.AsSearchCountries(["SE"]);

        SearchCountry searchCountry = searchCountries[0];

        searchCountry.Cca2.Should().Be("CA");
        searchCountry.Translations.Should().Contain(t => t.Code == "fra" && t.Name == "Canada");
        searchCountry.IsExcluded.Should().BeFalse();
    }

    [Fact]
    public void AsSearchCountryShouldHaveIsExcludedTrueWhenCountryIsExcluded()
    {
        Country[] countries = [_canada];

        SearchCountry[] searchCountries = countries.AsSearchCountries(["CA"]);

        SearchCountry searchCountry = searchCountries[0];

        searchCountry.IsExcluded.Should().BeTrue();
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = "CA",
        Capitals = [new Capital("Ottawa", new Coordinate(1, 2))],
        Area = new Area(9984670),
        Population = 36624199,
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(56.1304, -106.3468),
        Borders = ["USA"],
        Translations = [new Translation("fra", "Canada")]
    };
}
