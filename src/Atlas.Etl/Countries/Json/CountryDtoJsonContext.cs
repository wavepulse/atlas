// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using Atlas.Etl.Countries.Json.Converters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Atlas.Etl.Countries.Json;

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(CountryDto[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(CoordinateDtoJsonConverter), typeof(SubRegionDtoJsonConverter), typeof(TranslationDtoJsonConverter)],
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
internal sealed partial class CountryDtoJsonContext : JsonSerializerContext;
