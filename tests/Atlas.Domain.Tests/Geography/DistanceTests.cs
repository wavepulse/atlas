// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class DistanceTests
{
    private readonly Coordinate _canadaCoordinate = new(60.0, -95.0);
    private readonly Coordinate _italyCoordinate = new(42.83333333, 12.83333333);

    [Fact]
    public void DistanceShouldCalculateBetweenTwoCoordinates()
    {
        Distance distance = Distance.Calculate(_canadaCoordinate, _italyCoordinate);

        distance.Kilometers.Should().BeApproximately(6843.3, 0.5);
        distance.Miles.Should().BeApproximately(4252.2, 0.5);
    }

    [Fact]
    public void DistanceShouldReturnZeroWhenCoordinateAreSame()
    {
        Distance distance = Distance.Calculate(_canadaCoordinate, _canadaCoordinate);

        distance.Kilometers.Should().Be(0.0);
        distance.Miles.Should().Be(0.0);
    }

    [Fact]
    public void DistanceZeroShouldReturnZero()
    {
        Distance distance = Distance.Zero;

        distance.Kilometers.Should().Be(0.0);
        distance.Miles.Should().Be(0.0);
    }
}
