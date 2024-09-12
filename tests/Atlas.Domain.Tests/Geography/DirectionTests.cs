// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class DirectionTests
{
    private readonly Coordinate _canada = new(60, -95);
    private readonly Coordinate _italy = new(42.83333333, 12.83333333);
    private readonly Coordinate _fiji = new(-18, 175);

    [Fact]
    public void CalculateShouldReturnTheDirectionInAngleBetweenTwoCoordinates()
    {
        double direction = Direction.Calculate(_canada, _italy);

        direction.Should().Be(104.0);
    }

    [Fact]
    public void CalculateShouldReturnZeroGivenSameCoordinates()
    {
        double direction = Direction.Calculate(_canada, _canada);

        direction.Should().Be(0.0);
    }

    [Fact]
    public void DirectionShouldGiveTheAngleInTheThirdQuadrantGivenCanadaToFiji()
    {
        double direction = Direction.Calculate(_canada, _fiji);

        direction.Should().Be(223.0);
    }

    [Fact]
    public void DirectionShouldGiveTheAngleInTheFirstQuadrantGivenCanadaToFiji()
    {
        double direction = Direction.Calculate(_fiji, _canada);

        direction.Should().Be(43.0);
    }

    [Fact]
    public void DirectionShouldGiveTheAngleInTheFirstQuadrantGivenLongitudeFor180Degrees()
    {
        Coordinate from = new(0, 0);
        Coordinate to = new(0, 180);

        double direction = Direction.Calculate(from, to);

        direction.Should().Be(90);
    }
}
