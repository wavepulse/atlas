// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using System.Text;
using System.Text.Json;

namespace Atlas.Infrastructure.Json.Converters;

public sealed class AreaJsonConverterTests
{
    private readonly JsonSerializerOptions _options = new();
    private readonly AreaJsonConverter _converter = new();

    [Fact]
    public void ReadShouldReturnTheAreaFromJson()
    {
        Utf8JsonReader reader = CreateJsonReader();

        double area = _converter.Read(ref reader, typeof(Area), _options);

        area.Should().Be(42.0);
    }

    [Fact]
    public void WriteShouldWriteTheAreaToJson()
    {
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        writer.WriteStartObject();
        writer.WritePropertyName("area");

        _converter.Write(writer, new Area(42.0), _options);

        writer.WriteEndObject();
        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        json.Should().Be(/*lang=json,strict*/"""{"area":42}""");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        const string json = /*lang=json,strict*/"""{ "area": 42.0 }""";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.Number)
            reader.Read();

        return reader;
    }
}
