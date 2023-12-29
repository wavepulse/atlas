// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class DirectionTests
{
    [Fact]
    public void DirectionZeroShouldReturnZero()
    {
        Direction.Zero.Should().Be(0.0);
    }

    [Theory, ClassData(typeof(Coordinates))]
    public void DirectionShouldCalculateTheAngleWithTheCoordinates(Coordinate from, Coordinate to, double expectedDirection)
    {
        double direction = Direction.Calculate(from, to);

        direction.Should().Be(expectedDirection);
    }

    [Fact]
    public void DirectionShouldReturnZeroWhenCoordinatesAreSame()
    {
        Coordinate coordinate = new(60, -95);

        double direction = Direction.Calculate(coordinate, coordinate);

        direction.Should().Be(0.0);
    }
}

file sealed class Coordinates : TheoryData<Coordinate, Coordinate, double>
{
    public Coordinates()
    {
        Add(new Coordinate(35, 105), new Coordinate(36, 138), 87.0); // China to Japan
        Add(new Coordinate(60, -95), new Coordinate(36, 138), 253.0); // Canada to Japan
        Add(new Coordinate(60, -95), new Coordinate(38, -97), 183.0); // Canada to USA
        Add(new Coordinate(60, -95), new Coordinate(42.83333333, 12.83333333), 104.0); // Canada to Italy
        Add(new Coordinate(42.83333333, 12.83333333), new Coordinate(60, -95), 284.0); // Italy to Canada
        Add(new Coordinate(42.83333333, 12.83333333), new Coordinate(36, 138), 94.0); // Italy to Japan
        Add(new Coordinate(42.83333333, 12.83333333), new Coordinate(28, 3), 208.0); // Italy to Algeria
        Add(new Coordinate(60, -95), new Coordinate(-18, 175), 223.0); // Canada to Fiji
        Add(new Coordinate(42.83333333, 12.83333333), new Coordinate(-18, 175), 112.0); // Italy to Fiji
        Add(new Coordinate(-18, 175), new Coordinate(60, -95), 43.0); // Fiji to Canada
        Add(new Coordinate(-18, 175), new Coordinate(42.83333333, 12.83333333), 292.0); // Fiji to Italy
    }
}
