// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Atlas.Domain.Translations;

namespace Atlas.Domain.Flags;

public sealed record Flag
{
    public required FlagCode Code { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }

    public required Continent Continent { get; init; }

    public required Coordinate Coordinate { get; init; }

    public required Area Area { get; init; }
}
