// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Infrastructure.Json.Converters;

internal sealed class Cca2JsonConverter : JsonConverter<Cca2>
{
    public override Cca2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Cca2 value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}
