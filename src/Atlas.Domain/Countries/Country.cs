// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Atlas.Domain.Languages;

namespace Atlas.Domain.Countries;

public sealed record Country
{
    public required string Cca2 { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }

    public required IEnumerable<Capital> Capitals { get; init; }

    public required IEnumerable<string> Borders { get; init; }

    public required Continent Continent { get; init; }

    public required Coordinate Coordinate { get; init; }

    public required Area Area { get; init; }

    public required int Population { get; init; }
}
