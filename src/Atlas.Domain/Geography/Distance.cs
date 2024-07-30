// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Extensions;

namespace Atlas.Domain.Geography;

public sealed class Distance
{
    private const double EarthRadiusKilometers = 6371.0;
    private const double EarthRadiusMiles = 3958.8;

    private Distance(double kilometers, double miles)
    {
        Kilometers = kilometers;
        Miles = miles;
    }

    public double Kilometers { get; }

    public double Miles { get; }

    public static Distance Calculate(Coordinate from, Coordinate to)
    {
        if (from == to)
            return new(0.0, 0.0);

        double deltaLatitude = (to.Latitude - from.Latitude).ToRadians();
        double deltaLongitude = (to.Longitude - from.Longitude).ToRadians();
        double fromLatitude = from.Latitude.ToRadians();
        double toLatitude = to.Latitude.ToRadians();

        double sinLatitude = Math.Sin(deltaLatitude / 2);
        double sinLongitude = Math.Sin(deltaLongitude / 2);

        double a = (sinLatitude * sinLatitude) + (sinLongitude * sinLongitude * Math.Cos(fromLatitude) * Math.Cos(toLatitude));
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return new(c * EarthRadiusKilometers, c * EarthRadiusMiles);
    }
}
