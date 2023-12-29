// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Flags;
using Atlas.Domain.Geography;
using Atlas.Domain.Translations;

namespace Atlas.Application.Fakes;

internal static class FakeFlags
{
    internal static Flag Canada { get; } = new()
    {
        Code = new FlagCode("ca", "can"),
        Translations = [new Translation("fra", new Name("Canada", "Canada"))],
        Continent = Continent.America,
        Area = new Area(9984670),
        Coordinate = new Coordinate(60, -95)
    };
}
