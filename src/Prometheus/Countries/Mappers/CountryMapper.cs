// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Resources;
using Prometheus.Countries.Dto;
using System.Net.Mime;

namespace Prometheus.Countries.Mappers;

internal static class CountryMapper
{
    internal static Country[] AsDomain(this CountryDto[] dtos, IEnumerable<string> languages, IEnumerable<string> excludedCountries)
        => dtos.Select(dto => dto.AsDomain(languages, excludedCountries)).ToArray();

    private static Country AsDomain(this CountryDto dto, IEnumerable<string> languages, IEnumerable<string> excludedCountries) => new()
    {
        Cca2 = new Cca2(dto.Cca2),
        Capitals = dto.CapitalInfo.AsDomain(dto.Capitals, dto.Coordinate),
        Coordinate = dto.Coordinate.AsDomain(),
        Translations = dto.Translations.AsDomain(dto.Name, languages),
        Continent = dto.Region.AsDomain(dto.SubRegion),
        Area = new Area(dto.Area),
        Borders = dto.Borders ?? [],
        Population = dto.Population,
        IsExcluded = excludedCountries.Contains(dto.Cca2, StringComparer.OrdinalIgnoreCase),
        Resources = new CountryResources()
        {
            Flag = new Image(dto.Flags.Svg, MediaTypeNames.Image.Svg),
            Map = dto.Maps.GoogleMaps
        }
    };
}
