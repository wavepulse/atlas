// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

public sealed class ContinentMapperTests
{
    [Fact]
    public void AsDomainShouldThrowExceptionWhenRegionIsUnknown()
    {
        Action action = () => ((RegionDto)999).AsDomain(subRegion: null);
        action.Should().Throw<ArgumentException>().WithMessage("Unknown region or sub region: 999 -  (Parameter 'region')");
    }

    [Theory]
    [InlineData(RegionDto.Africa, Continent.Africa)]
    [InlineData(RegionDto.Asia, Continent.Asia)]
    [InlineData(RegionDto.Europe, Continent.Europe)]
    [InlineData(RegionDto.Oceania, Continent.Oceania)]
    [InlineData(RegionDto.Antarctic, Continent.Antarctica)]
    internal void AsDomainShouldMapDtoToDomain(RegionDto dto, Continent expectedContinent)
    {
        Continent continent = dto.AsDomain(subRegion: null);

        continent.Should().Be(expectedContinent);
    }

    [Theory]
    [InlineData(RegionDto.Americas, SubRegionDto.NorthAmerica, Continent.NorthAmerica)]
    [InlineData(RegionDto.Americas, SubRegionDto.SouthAmerica, Continent.SouthAmerica)]
    [InlineData(RegionDto.Americas, SubRegionDto.CentralAmerica, Continent.CentralAmerica)]
    [InlineData(RegionDto.Americas, SubRegionDto.Caribbean, Continent.CentralAmerica)]
    internal void AsDomainShouldMapDtoToDomainWithSubRegion(RegionDto region, SubRegionDto subRegion, Continent expectedContinent)
    {
        Continent continent = region.AsDomain(subRegion);

        continent.Should().Be(expectedContinent);
    }
}
