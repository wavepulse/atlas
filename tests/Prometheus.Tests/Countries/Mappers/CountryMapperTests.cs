// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Prometheus.Countries.Dto;
using System.Net.Mime;

namespace Prometheus.Countries.Mappers;

public sealed class CountryMapperTests
{
    private readonly CountryDto _dto = new()
    {
        Cca2 = "CA",
        Name = new NameDto("Canada"),
        Capitals = ["Ottawa"],
        Borders = ["USA"],
        Area = 9984670,
        Population = 38041000,
        Coordinate = new CoordinateDto(60.0, 95.0),
        Region = RegionDto.Americas,
        SubRegion = SubRegionDto.NorthAmerica,
        CapitalInfo = new CapitalInfoDto { Coordinate = new CoordinateDto(45.4215, 75.6972) },
        Translations = [new TranslationDto("fra", "Canada")],
        Maps = new MapsDto(new Uri("https://www.google.com/maps/place/Canada")),
        Flags = new FlagsDto(new Uri("https://www.countryflags.io/ca/flat/64.png"))
    };

    [Fact]
    public void AsDomainShouldMapDtoToDomain()
    {
        CountryDto[] dtos = [_dto];

        Country[] countries = dtos.AsDomain(["fra", "eng"], []);

        Country country = countries[0];

        country.Cca2.Value.Should().Be("CA");
        country.Borders.Should().Contain(_dto.Borders);
        country.Population.Should().Be(_dto.Population);
        country.Continent.Should().Be(Continent.NorthAmerica);

        double area = country.Area;
        area.Should().Be(_dto.Area);

        country.Coordinate.Latitude.Should().Be(_dto.Coordinate.Latitude);
        country.Coordinate.Longitude.Should().Be(_dto.Coordinate.Longitude);

        Capital capital = country.Capitals.First();
        capital.Name.Should().Be("Ottawa");
        capital.Coordinate.Latitude.Should().Be(_dto.CapitalInfo.Coordinate!.Latitude);
        capital.Coordinate.Longitude.Should().Be(_dto.CapitalInfo.Coordinate.Longitude);

        country.Translations.Should().Contain(t => t.Language == Language.French && t.Name == "Canada");
        country.Translations.Should().Contain(t => t.Language == Language.English && t.Name == "Canada");

        country.IsExcluded.Should().BeFalse();

        country.Resources.Map.Should().Be(new Uri("https://www.google.com/maps/place/Canada"));
        country.Resources.Flag.Uri.Should().Be(new Uri("https://www.countryflags.io/ca/flat/64.png"));
        country.Resources.Flag.MediaType.Should().Be(MediaTypeNames.Image.Svg);
    }

    [Fact]
    public void DtoWhichIsExcludedShouldMapToDomainWithIsExcluded()
    {
        CountryDto[] dtos = [_dto];

        Country[] countries = dtos.AsDomain(["fra", "eng"], [_dto.Cca2]);

        Country country = countries[0];

        country.IsExcluded.Should().BeTrue();
    }

    [Fact]
    public void DtoWithoutBordersShouldMapToEmptyArray()
    {
        CountryDto countryWithoutBorder = _dto with { Borders = null };

        CountryDto[] dtos = [countryWithoutBorder];

        Country[] countries = dtos.AsDomain(["fra", "eng"], []);

        Country country = countries[0];

        country.Borders.Should().BeEmpty();
    }
}
