// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

internal static class CoordinateMapper
{
    internal static Coordinate AsDomain(this CoordinateDto dto) => new(dto.Latitude, dto.Longitude);
}
