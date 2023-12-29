// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Extensions;

namespace Atlas.Domain.Geography;

public sealed class Distance
{
    private Distance(double kilometers, double miles)
    {
        Kilometers = kilometers;
        Miles = miles;
    }

    public static Distance Zero { get; } = new(0, 0);

    public double Kilometers { get; }

    public double Miles { get; }

    /// <summary>
    /// Calculate the distance between two coordinates based on Haversine formula.
    /// <para><seealso href="https://www.movable-type.co.uk/scripts/latlong.html"/></para>
    /// </summary>
    /// <param name="from">the coordinate of from.</param>
    /// <param name="to">the coordinate of to.</param>
    /// <returns>The distance between two coordinates.</returns>
    public static Distance Calculate(Coordinate from, Coordinate to)
    {
        if (from == to)
            return Zero;

        const double earthRadiusInMiles = 3958.8;
        const double earthRadiusInKilometers = 6371.0;

        double deltaLatitude = (to.Latitude - from.Latitude).ToRadians();
        double deltaLongitude = (to.Longitude - from.Longitude).ToRadians();
        double fromLatitude = from.Latitude.ToRadians();
        double toLatitude = to.Latitude.ToRadians();

        double sinLatitude = Math.Sin(deltaLatitude / 2);
        double sinLongitude = Math.Sin(deltaLongitude / 2);

        double a = (sinLatitude * sinLatitude) + (sinLongitude * sinLongitude * Math.Cos(fromLatitude) * Math.Cos(toLatitude));
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return new(earthRadiusInKilometers * c, earthRadiusInMiles * c);
    }
}
