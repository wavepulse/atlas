// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Etl.Countries.Json.Converters;

internal sealed class SubRegionDtoJsonConverter : JsonConverter<SubRegionDto?>
{
    public override SubRegionDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? subRegion = reader.GetString();

        return subRegion switch
        {
            "North America" => SubRegionDto.NorthAmerica,
            "South America" => SubRegionDto.SouthAmerica,
            "Central America" => SubRegionDto.CentralAmerica,
            "Caribbean" => SubRegionDto.Caribbean,
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter writer, SubRegionDto? value, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(SubRegionDto)} is only used for deserialization");
}
