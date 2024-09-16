// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json.Serialization;

namespace Prometheus.Countries.Dto;

internal sealed record CapitalInfoDto
{
    [JsonPropertyName("latlng")]
    public CoordinateDto? Coordinate { get; init; }
}
