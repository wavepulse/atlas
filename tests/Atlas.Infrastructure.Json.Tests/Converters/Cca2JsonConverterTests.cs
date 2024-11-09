// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using System.Text;
using System.Text.Json;

namespace Atlas.Infrastructure.Json.Converters;

public sealed class Cca2JsonConverterTests
{
    private readonly JsonSerializerOptions _options = new();
    private readonly Cca2JsonConverter _converter = new();

    [Fact]
    public void ReadShouldReturnTheCca2FromJson()
    {
        Utf8JsonReader reader = CreateJsonReader();

        Cca2 cca2 = _converter.Read(ref reader, typeof(Cca2), _options);

        cca2.Should().Be(new Cca2("CA"));
    }

    [Fact]
    public void WriteShouldWriteTheCca2ToJson()
    {
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        writer.WriteStartObject();
        writer.WritePropertyName("cca2");

        _converter.Write(writer, new Cca2("CA"), _options);

        writer.WriteEndObject();
        writer.Flush();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        json.Should().Be(/*lang=json,strict*/"""{"cca2":"CA"}""");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        const string json = /*lang=json,strict*/"""{ "cca2": "CA" }""";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.String)
            reader.Read();

        return reader;
    }
}
