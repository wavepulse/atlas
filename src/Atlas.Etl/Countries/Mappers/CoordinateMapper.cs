// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Mappers;

internal static class CoordinateMapper
{
    internal static Coordinate AsDomain(this CoordinateDto dto) => new(dto.Latitude, dto.Longitude);
}
