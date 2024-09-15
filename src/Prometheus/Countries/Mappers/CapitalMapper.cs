// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

internal static class CapitalMapper
{
    internal static Capital[] AsDomain(this CapitalInfoDto dto, IEnumerable<string>? capitals, CoordinateDto fallbackCoordinate)
    {
        if (capitals is null)
            return [];

        Coordinate coordinate = (dto.Coordinate ?? fallbackCoordinate).AsDomain();

        return capitals.Select(capital => new Capital(capital, coordinate)).ToArray();
    }
}
