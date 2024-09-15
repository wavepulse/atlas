// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Contracts.Flags;

public sealed record GuessedFlag
{
    public required string Cca2 { get; init; }

    public required string Name { get; init; }

    public required bool IsSameContinent { get; init; }

    public required int Kilometers { get; init; }

    public required double Direction { get; init; }

    public required bool Success { get; init; }

    public required Uri FlagSvgUri { get; init; }
}
