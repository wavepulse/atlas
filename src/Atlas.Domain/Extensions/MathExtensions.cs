// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Extensions;

internal static class MathExtensions
{
    internal static double ToRadians(this double degree) => degree * (Math.PI / 180.0);

    internal static double ToDegrees(this double radians) => radians * (180.0 / Math.PI);

    internal static double Normalize(this double angle) => (angle + 360) % 360;
}
