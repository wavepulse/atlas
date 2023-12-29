// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using AreaSizeContract = Atlas.Contracts.Geography.AreaSize;

namespace Atlas.Application.Geography.Mappers;

internal static class AreaSizeMapper
{
    internal static AreaSizeContract AsContract(this AreaSize areaSize) => areaSize switch
    {
        AreaSize.Smaller => AreaSizeContract.Smaller,
        AreaSize.Same => AreaSizeContract.Same,
        AreaSize.Larger => AreaSizeContract.Larger,
        _ => AreaSizeContract.Same,
    };
}
