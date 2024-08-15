// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using System.Text;
using System.Text.Json;

namespace Atlas.Etl.Countries.Json.Converters;

public sealed class SubRegionDtoJsonConverterTests
{
    private readonly JsonSerializerOptions _options = new();

    private readonly SubRegionDtoJsonConverter _converter = new();

    [Fact]
    public void ReadShouldReturnNorthAmericaAsSubRegion()
    {
        const string json = /*lang=json,strict*/"""{ "subregion": "North America" }""";

        Utf8JsonReader reader = CreateJsonReader(json);

        SubRegionDto? subRegion = _converter.Read(ref reader, typeof(SubRegionDto), _options);

        subRegion.Should().Be(SubRegionDto.NorthAmerica);
    }

    [Fact]
    public void ReadShouldReturnSouthAmericaAsSubRegion()
    {
        const string json = /*lang=json,strict*/"""{ "subregion": "South America" }""";

        Utf8JsonReader reader = CreateJsonReader(json);

        SubRegionDto? subRegion = _converter.Read(ref reader, typeof(SubRegionDto), _options);

        subRegion.Should().Be(SubRegionDto.SouthAmerica);
    }

    [Fact]
    public void ReadShouldReturnNullWhenDoesNotRecognizeSubRegion()
    {
        const string json = /*lang=json,strict*/"""{ "subregion": "Polynesia" }""";

        Utf8JsonReader reader = CreateJsonReader(json);

        SubRegionDto? subRegion = _converter.Read(ref reader, typeof(SubRegionDto), _options);

        subRegion.Should().BeNull();
    }

    [Fact]
    public void WriteShouldThrowNotSupportedException()
    {
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        Action callback = () => _converter.Write(writer, value: null, _options);

        callback.Should().ThrowExactly<NotSupportedException>()
                         .WithMessage($"{nameof(SubRegionDto)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader(string json)
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.String)
            reader.Read();

        return reader;
    }
}
