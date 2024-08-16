// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Mappers;

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
        Translations = [new TranslationDto("fra", "Canada")]
    };

    [Fact]
    public void AsDomainShouldMapDtoToDomain()
    {
        CountryDto[] dtos = [_dto];

        Country[] countries = dtos.AsDomain(["fra", "eng"]);

        Country country = countries[0];

        country.Cca2.Should().Be("CA");
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

        country.Translations.Should().Contain(t => t.Code == "fra" && t.Name == "Canada");
        country.Translations.Should().Contain(t => t.Code == "eng" && t.Name == "Canada");
    }

    [Fact]
    public void DtoWithoutBordersShouldMapToEmptyArray()
    {
        CountryDto countryWithoutBorder = _dto with { Borders = null };

        CountryDto[] dtos = [countryWithoutBorder];

        Country[] countries = dtos.AsDomain(["fra", "eng"]);

        Country country = countries[0];

        country.Borders.Should().BeEmpty();
    }
}
