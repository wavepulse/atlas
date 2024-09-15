// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Mappers;

internal static class CountryMapper
{
    internal static Country[] AsDomain(this CountryDto[] dtos, IEnumerable<string> languages) => dtos.Select(dto => dto.AsDomain(languages)).ToArray();

    private static Country AsDomain(this CountryDto dto, IEnumerable<string> languages) => new()
    {
        Cca2 = dto.Cca2,
        Capitals = dto.CapitalInfo.AsDomain(dto.Capitals, dto.Coordinate),
        Coordinate = dto.Coordinate.AsDomain(),
        Translations = dto.Translations.AsDomain(dto.Name, languages),
        Continent = dto.Region.AsDomain(dto.SubRegion),
        Area = new Area(dto.Area),
        Borders = dto.Borders ?? [],
        Population = dto.Population,
        MapUri = dto.Maps.GoogleMaps,
        FlagSvgUri = dto.Flags.Svg
    };
}
