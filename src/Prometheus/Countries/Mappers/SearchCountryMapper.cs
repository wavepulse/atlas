// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Prometheus.Countries.Mappers;

internal static class SearchCountryMapper
{
    internal static SearchCountry[] AsSearchCountries(this Country[] countries, IEnumerable<string> excludedCountries)
        => countries.Select(country => country.AsSearchCountry(excludedCountries.Contains(country.Cca2, StringComparer.OrdinalIgnoreCase))).ToArray();

    private static SearchCountry AsSearchCountry(this Country country, bool isExcluded) => new()
    {
        Cca2 = country.Cca2,
        Translations = country.Translations,
        IsExcluded = isExcluded
    };
}
