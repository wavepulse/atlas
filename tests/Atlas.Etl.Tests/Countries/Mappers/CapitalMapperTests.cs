// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Mappers;

public sealed class CapitalMapperTests
{
    private readonly CapitalInfoDto _capitalDto = new() { Coordinate = new CoordinateDto(42, 42) };
    private readonly CoordinateDto _coordinateDto = new(0, 90);

    [Fact]
    public void AsDomainShouldMapDtoToDomain()
    {
        Capital[] capitals = _capitalDto.AsDomain(["Ottawa"], _coordinateDto);

        Capital capital = capitals[0];

        capital.Name.Should().Be("Ottawa");
        capital.Coordinate.Latitude.Should().Be(_capitalDto.Coordinate!.Latitude);
        capital.Coordinate.Longitude.Should().Be(_capitalDto.Coordinate.Longitude);
    }

    [Fact]
    public void AsDomainShouldReturnEmptyWhenThereIsNoCapitals()
    {
        CapitalInfoDto dto = new();

        Capital[] capitals = dto.AsDomain(capitals: null, _coordinateDto);

        capitals.Should().BeEmpty();
    }

    [Fact]
    public void AsDomainShouldMapWithFallbackCoordinateWhenCapitalInfoCoordinateIsNull()
    {
        CapitalInfoDto capitalInfoWithoutCoordinate = _capitalDto with { Coordinate = null };

        Capital[] capitals = capitalInfoWithoutCoordinate.AsDomain(["Ottawa"], _coordinateDto);

        Capital capital = capitals[0];

        capital.Name.Should().Be("Ottawa");
        capital.Coordinate.Latitude.Should().Be(_coordinateDto.Latitude);
        capital.Coordinate.Longitude.Should().Be(_coordinateDto.Longitude);
    }
}
