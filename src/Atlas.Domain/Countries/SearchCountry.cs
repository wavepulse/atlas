// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;

namespace Atlas.Domain.Countries;

public sealed record SearchCountry
{
    public required string Cca2 { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }

    public bool IsExcluded { get; init; }
}
