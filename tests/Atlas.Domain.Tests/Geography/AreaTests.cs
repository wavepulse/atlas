// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class AreaTests
{
    [Fact]
    public void CompareToShouldReturnSmallerWhenLeftIsSmallerThanRightArea()
    {
        Area left = new(0.0);
        Area right = new(1.0);

        Area.Size size = left.CompareTo(right);

        size.Should().Be(Area.Size.Smaller);
    }

    [Fact]
    public void CompareToShouldReturnEqualWhenLeftIsEqualToRightArea()
    {
        Area left = new(1.0);
        Area right = new(1.0);

        Area.Size size = left.CompareTo(right);

        size.Should().Be(Area.Size.Same);
    }

    [Fact]
    public void CompareToShouldReturnGreaterWhenLeftAreaIsGreaterThanRightArea()
    {
        Area left = new(1.0);
        Area right = new(0.0);

        Area.Size size = left.CompareTo(right);

        size.Should().Be(Area.Size.Larger);
    }
}
