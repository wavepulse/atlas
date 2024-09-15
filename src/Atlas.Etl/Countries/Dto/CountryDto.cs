// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json.Serialization;

namespace Atlas.Etl.Countries.Dto;

internal sealed record CountryDto
{
    public required string Cca2 { get; init; }

    public required NameDto Name { get; init; }

    [JsonPropertyName("capital")]
    public IEnumerable<string>? Capitals { get; init; }

    public required RegionDto Region { get; init; }

    public SubRegionDto? SubRegion { get; init; }

    public IEnumerable<TranslationDto> Translations { get; init; } = [];

    [JsonPropertyName("latlng")]
    public required CoordinateDto Coordinate { get; init; }

    public IEnumerable<string>? Borders { get; init; }

    public required double Area { get; init; }

    public required int Population { get; init; }

    public required CapitalInfoDto CapitalInfo { get; init; }

    public required MapsDto Maps { get; init; }

    public required FlagsDto Flags { get; init; }
}
