// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using System.Text;
using System.Text.Json;

namespace Prometheus.Countries.Json.Converters;

public sealed class TranslationDtoJsonConverterTests
{
    private const string Json = /*lang=json,strict*/"""{ "translations": { "fra": { "official": "Official", "common": "Canada" }, "swe": { "official": "Official", "common": "Kanada" } } }""";

    private readonly JsonSerializerOptions _options = new();

    private readonly TranslationDtoJsonConverter _converter = new();

    [Fact]
    public void ReadShouldReturnAListOfTranslations()
    {
        Utf8JsonReader reader = CreateJsonReader(Json);

        IEnumerable<TranslationDto> translations = _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        translations.Should().HaveCount(2);

        translations.Should().Contain(t => t == new TranslationDto("fra", "Canada"));
        translations.Should().Contain(t => t == new TranslationDto("swe", "Kanada"));
    }

    [Fact]
    public void ReadShouldReturnEmptyListWhenThereIsNoTranslations()
    {
        const string json = /*lang=json,strict*/"""{ "translations": {} }""";

        Utf8JsonReader reader = CreateJsonReader(json);

        IEnumerable<TranslationDto> translations = _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        translations.Should().BeEmpty();
    }

    [Fact]
    public void ReadShouldStopReadingAtEndObject()
    {
        Utf8JsonReader reader = CreateJsonReader(Json);

        _ = _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        reader.TokenType.Should().Be(JsonTokenType.EndObject);
    }

    [Fact]
    public void WriteShouldThrowNotSupportedException()
    {
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        Action callback = () => _converter.Write(writer, [], _options);

        callback.Should().ThrowExactly<NotSupportedException>()
                         .WithMessage($"{nameof(TranslationDto)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader(string json)
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        reader.Read();
        reader.Read();

        while (reader.TokenType != JsonTokenType.StartObject)
            reader.Read();

        return reader;
    }
}
