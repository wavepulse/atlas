// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Infrastructure.Json.Converters;

internal sealed class AreaJsonConverter : JsonConverter<Area>
{
    public override Area Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new(reader.GetDouble());

    public override void Write(Utf8JsonWriter writer, Area value, JsonSerializerOptions options) => writer.WriteNumberValue(value);
}
