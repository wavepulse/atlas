// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using Atlas.Etl.Countries.Json.Converters;
using System.Text.Json.Serialization;

namespace Atlas.Etl.Countries.Json;

[JsonSerializable(typeof(CountryDto[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(CoordinateDtoJsonConverter), typeof(SubRegionDtoJsonConverter), typeof(TranslationDtoJsonConverter)],
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
internal sealed partial class CountryDtoJsonContext : JsonSerializerContext;
