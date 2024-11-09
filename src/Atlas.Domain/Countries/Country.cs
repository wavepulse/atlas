// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Domain.Countries;

public sealed record Country
{
    public required Cca2 Cca2 { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }

    public required IEnumerable<Capital> Capitals { get; init; }

    public required IEnumerable<string> Borders { get; init; }

    public required Continent Continent { get; init; }

    public required Coordinate Coordinate { get; init; }

    public required Area Area { get; init; }

    public required int Population { get; init; }

    public required CountryResources Resources { get; init; }

    public required bool IsExcluded { get; init; }
}
