// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json.Converters;
using System.Text.Json.Serialization;

namespace Prometheus.Countries.Json;

[JsonSerializable(typeof(CountryDto[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(CoordinateDtoJsonConverter), typeof(SubRegionDtoJsonConverter), typeof(TranslationDtoJsonConverter)],
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
internal sealed partial class CountryDtoJsonContext : JsonSerializerContext;
