// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class DistanceTests
{
    private readonly Coordinate _canada = new(60.0, -95.0);
    private readonly Coordinate _italy = new(42.83333333, 12.83333333);

    [Fact]
    public void CalculateShouldReturnTheDistanceBetweenTwoCoordinates()
    {
        Distance distance = Distance.Calculate(_canada, _italy);

        distance.Kilometers.Should().BeApproximately(6843.3, 0.1);
        distance.Miles.Should().BeApproximately(4252.2, 0.1);
    }

    [Fact]
    public void CalculateShouldReturnZeroGivenSameCoordinates()
    {
        Distance distance = Distance.Calculate(_canada, _canada);

        distance.Kilometers.Should().Be(0.0);
        distance.Miles.Should().Be(0.0);
    }
}
