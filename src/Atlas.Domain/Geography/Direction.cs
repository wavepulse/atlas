// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Extensions;

namespace Atlas.Domain.Geography;

internal static class Direction
{
    public static double Calculate(Coordinate from, Coordinate to)
    {
        if (from == to)
            return 0;

        double deltaLatitude = CalculateDeltaLatitude(from.Latitude, to.Latitude);
        double deltaLongitude = CalculateDeltaLongitude(from.Longitude, to.Longitude);

        double bearing = Math.Floor(Math.Atan2(deltaLongitude, deltaLatitude).ToDegrees());

        return bearing.Normalize();
    }

    /// <summary>
    /// Calculate Δ latitude.
    /// In the calculation of the rhumb line, the use of Math.PI / 4 is related to the Mercator projection,
    /// which is used to convert between geographic latitude (in degrees) and the Mercator projection's auxiliary latitude.
    /// </summary>
    /// <param name="fromLatitude">from latitude.</param>
    /// <param name="toLatitude">to latitude.</param>
    /// <returns>The calculated Δ latitude.</returns>
    private static double CalculateDeltaLatitude(double fromLatitude, double toLatitude)
    {
        const double auxiliaryLatitude = Math.PI / 4;

        double fromTangent = Math.Tan(auxiliaryLatitude + (fromLatitude.ToRadians() / 2));
        double toTangent = Math.Tan(auxiliaryLatitude + (toLatitude.ToRadians() / 2));

        return Math.Log(toTangent / fromTangent);
    }

    /// <summary>
    /// Calculate Δ longitude.
    /// If Δ longitude is over 180°, take the shorter rhumb line across the anti-meridian.
    /// </summary>
    /// <param name="fromLongitude">from longitude.</param>
    /// <param name="toLongitude">to longitude.</param>
    /// <returns>the calculated Δ longitude.</returns>
    private static double CalculateDeltaLongitude(double fromLongitude, double toLongitude)
    {
        double deltaLongitude = (toLongitude - fromLongitude).ToRadians();

        return Math.Abs(deltaLongitude) > Math.PI
            ? GetShorterRhumbLine(deltaLongitude)
            : deltaLongitude;

        static double GetShorterRhumbLine(double deltaLongitude)
        {
            return deltaLongitude > 0
                ? -((2 * Math.PI) - deltaLongitude)
                : (2 * Math.PI) + deltaLongitude;
        }
    }
}
