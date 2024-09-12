// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using System.Text;
using System.Text.Json;

namespace Atlas.Etl.Countries.Json.Converters;

public sealed class CoordinateDtoJsonConverterTests
{
    private const string Json = /*lang=json,strict*/"""{ "latlng": [ 42.83333333, 12.83333333 ] }""";

    private readonly JsonSerializerOptions _options = new();

    private readonly CoordinateDtoJsonConverter _converter = new();

    [Fact]
    public void ReadShouldReturnTheLatitude()
    {
        Utf8JsonReader reader = CreateJsonReader();

        (double latitude, _) = _converter.Read(ref reader, typeof(CoordinateDto), _options);

        latitude.Should().Be(42.83333333);
    }

    [Fact]
    public void ReadShouldReturnTheLongitude()
    {
        Utf8JsonReader reader = CreateJsonReader();

        (_, double longitude) = _converter.Read(ref reader, typeof(CoordinateDto), _options);

        longitude.Should().Be(12.83333333);
    }

    [Fact]
    public void ReadShouldStopReadingAtEndArray()
    {
        Utf8JsonReader reader = CreateJsonReader();

        _ = _converter.Read(ref reader, typeof(CoordinateDto), _options);

        reader.TokenType.Should().Be(JsonTokenType.EndArray);
    }

    [Fact]
    public void WriteShouldThrowNotSupportedException()
    {
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        Action callback = () => _converter.Write(writer, new CoordinateDto(0, 0), _options);

        callback.Should().ThrowExactly<NotSupportedException>()
                         .WithMessage($"{nameof(CoordinateDto)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(Json));

        while (reader.TokenType != JsonTokenType.StartArray)
            reader.Read();

        return reader;
    }
}
