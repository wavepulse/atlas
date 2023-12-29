// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using AreaSizeContract = Atlas.Contracts.Geography.AreaSize;

namespace Atlas.Application.Geography.Mappers;

public sealed class AreaSizeMapperTests
{
    [Theory, ClassData(typeof(AreaSizes))]
    public void AsContractShouldConvertDomainToContract(AreaSize areaSize, AreaSizeContract expectedSize)
    {
        AreaSizeContract size = areaSize.AsContract();

        size.Should().Be(expectedSize);
    }

    [Fact]
    public void AsContractShouldReturnSameWhenReceiveAnUnknownAreaSize()
    {
        AreaSizeContract areaSize = ((AreaSize)42).AsContract();

        areaSize.Should().Be(AreaSizeContract.Same);
    }

    internal sealed class AreaSizes : TheoryData<AreaSize, AreaSizeContract>
    {
        public AreaSizes()
        {
            Add(AreaSize.Smaller, AreaSizeContract.Smaller);
            Add(AreaSize.Same, AreaSizeContract.Same);
            Add(AreaSize.Larger, AreaSizeContract.Larger);
        }
    }
}
