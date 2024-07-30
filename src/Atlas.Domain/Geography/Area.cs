// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class Area(double area)
{
    public enum Size
    {
        Smaller = -1,
        Same = 0,
        Larger = 1
    }

    public static implicit operator double(Area current) => current.ToDouble();

    public Size CompareTo(Area other)
    {
        if (Math.Abs(area - other) < double.Epsilon)
            return Size.Same;

        return area < other ? Size.Smaller : Size.Larger;
    }

    private double ToDouble() => area;
}
