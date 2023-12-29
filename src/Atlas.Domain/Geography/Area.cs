// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class Area(double area)
{
    public static implicit operator double(Area area) => area.ToDouble();

    public AreaSize CompareTo(Area other)
    {
        if (area == other)
            return AreaSize.Same;

        return area > other
            ? AreaSize.Larger
            : AreaSize.Smaller;
    }

    public double ToDouble() => area;
}
