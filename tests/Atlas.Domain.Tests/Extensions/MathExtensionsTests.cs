// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Extensions;

public sealed class MathExtensionsTests
{
    [Fact]
    public void ToRadiansShouldConvertDegreesToRadians()
    {
        const double degree = 180.0;

        double actual = degree.ToRadians();

        actual.Should().Be(Math.PI);
    }

    [Fact]
    public void ToDegreesShouldConvertRadiansToDegrees()
    {
        const double radian = Math.PI;

        double actual = radian.ToDegrees();

        actual.Should().Be(180.0);
    }

    [Theory]
    [InlineData(-270, 90.0)]
    [InlineData(-180, 180.0)]
    [InlineData(-90, 270.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(90.0, 90.0)]
    [InlineData(180.0, 180.0)]
    [InlineData(360.0, 0.0)]
    public void NormalizeShouldAdaptTheAngleIn360Degrees(double angle, double normalizedAngle)
    {
        double actual = angle.Normalize();

        actual.Should().Be(normalizedAngle);
    }
}
