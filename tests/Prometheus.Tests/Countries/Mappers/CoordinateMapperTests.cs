// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

public sealed class CoordinateMapperTests
{
    [Fact]
    public void AsDomainShouldMapDtoToDomain()
    {
        CoordinateDto dto = new(1.0, 2.0);
        Coordinate coordinate = dto.AsDomain();

        coordinate.Latitude.Should().Be(1.0);
        coordinate.Longitude.Should().Be(2.0);
    }
}
