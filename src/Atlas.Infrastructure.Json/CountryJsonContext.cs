// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json.Converters;
using System.Text.Json.Serialization;

namespace Atlas.Infrastructure.Json;

[JsonSerializable(typeof(Country[]))]
[JsonSerializable(typeof(SearchCountry[]))]
[JsonSerializable(typeof(CountryLookup[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(AreaJsonConverter), typeof(Cca2JsonConverter)],
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
public sealed partial class CountryJsonContext : JsonSerializerContext;
