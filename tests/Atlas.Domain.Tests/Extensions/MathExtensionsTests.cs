// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Extensions;

public sealed class MathExtensionsTests
{
    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(45.0, 0.785398)]
    [InlineData(90, 1.5708)]
    [InlineData(180, 3.14159)]
    public void ToRadiansShouldConvertAnAngleInDegreesToRadians(double degrees, double expectedRadians)
    {
        double radians = degrees.ToRadians();

        radians.Should().BeApproximately(expectedRadians, 0.5);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(0.785398, 45.0)]
    [InlineData(1.5708, 90)]
    [InlineData(3.14159, 180)]
    public void ToDegreesShouldConvertAnAngleInRadiansToDegrees(double radians, double expectedDegrees)
    {
        double degrees = radians.ToDegrees();

        degrees.Should().BeApproximately(expectedDegrees, 0.5);
    }

    [Theory]
    [InlineData(-360, 0.0)]
    [InlineData(-270, 90.0)]
    [InlineData(-180, 180.0)]
    [InlineData(-90, 270.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(90, 90)]
    [InlineData(180, 180)]
    [InlineData(360, 0)]
    public void NormalizeShouldAdaptTheAngleIn360Degrees(double angle, double expectedNormalizedAngle)
    {
        double normalizedAngle = angle.Normalize();

        normalizedAngle.Should().BeApproximately(expectedNormalizedAngle, 0.5);
    }
}
