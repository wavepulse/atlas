// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class AreaTests
{
    [Fact]
    public void ImplicitConversionShouldReturnTheArea()
    {
        double area = new Area(42.0);

        area.Should().Be(42.0);
    }

    [Theory, ClassData(typeof(AreaSizes))]
    public void CompareToShouldReturnTheGoodAreaSize(Area left, Area right, AreaSize expectedSize)
    {
        AreaSize size = left.CompareTo(right);

        size.Should().Be(expectedSize);
    }

    internal sealed class AreaSizes : TheoryData<Area, Area, AreaSize>
    {
        public AreaSizes()
        {
            Add(new Area(0.0), new Area(0.0), AreaSize.Same);
            Add(new Area(0.0), new Area(1.0), AreaSize.Smaller);
            Add(new Area(1.0), new Area(0.0), AreaSize.Larger);
        }
    }
}
