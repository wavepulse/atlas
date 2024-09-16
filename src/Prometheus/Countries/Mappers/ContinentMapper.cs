// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

internal static class ContinentMapper
{
    internal static Continent AsDomain(this RegionDto region, SubRegionDto? subRegion) => (region, subRegion) switch
    {
        (RegionDto.Africa, null) => Continent.Africa,
        (RegionDto.Asia, null) => Continent.Asia,
        (RegionDto.Europe, null) => Continent.Europe,
        (RegionDto.Oceania, null) => Continent.Oceania,
        (RegionDto.Antarctic, null) => Continent.Antarctica,
        (RegionDto.Americas, SubRegionDto.NorthAmerica) => Continent.NorthAmerica,
        (RegionDto.Americas, SubRegionDto.SouthAmerica) => Continent.SouthAmerica,
        (RegionDto.Americas, SubRegionDto.CentralAmerica) => Continent.CentralAmerica,
        (RegionDto.Americas, SubRegionDto.Caribbean) => Continent.CentralAmerica,
        _ => throw new ArgumentException($"Unknown region or sub region: {region} - {subRegion}", nameof(region))
    };
}
