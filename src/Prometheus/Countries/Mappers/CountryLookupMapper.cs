// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Prometheus.Countries.Mappers;

internal static class CountryLookupMapper
{
    internal static CountryLookup[] AsLookups(this Country[] countries)
        => countries.Select(AsLookup).ToArray();

    private static CountryLookup AsLookup(this Country country) => new()
    {
        Cca2 = country.Cca2,
        Translations = country.Translations,
        IsExcluded = country.IsExcluded
    };
}
