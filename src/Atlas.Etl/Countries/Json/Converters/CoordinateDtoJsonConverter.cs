// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Etl.Countries.Json.Converters;

internal sealed class CoordinateDtoJsonConverter : JsonConverter<CoordinateDto>
{
    public override CoordinateDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        double latitude = SkipPropertyAndGetValue(ref reader);
        double longitude = SkipPropertyAndGetValue(ref reader);

        _ = reader.Read();

        return new CoordinateDto(latitude, longitude);

        static double SkipPropertyAndGetValue(ref Utf8JsonReader reader)
        {
            _ = reader.Read();
            return reader.GetDouble();
        }
    }

    public override void Write(Utf8JsonWriter writer, CoordinateDto value, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(CoordinateDto)} is only used for deserialization");
}
