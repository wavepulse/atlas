// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json.Converters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Atlas.Infrastructure.Json;

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(Country[]))]
[JsonSerializable(typeof(SearchCountry[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(AreaJsonConverter)],
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
public sealed partial class CountryJsonContext : JsonSerializerContext;
