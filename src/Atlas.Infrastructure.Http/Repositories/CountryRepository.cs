// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Http.Caching;
using Atlas.Infrastructure.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Atlas.Infrastructure.Http.Repositories;

internal sealed class CountryRepository(HttpClient client, IAppCache appCache) : ICountryRepository
{
    private const string CountriesKey = "country:codes";

    public async Task<Country[]> GetAllAsync(CancellationToken cancellationToken)
    {
        if (appCache.TryGetValue(CountriesKey, out Country[]? cachedCountries))
            return cachedCountries;

        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.Countries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken)
                                                     .ConfigureAwait(false);

        using ICacheEntry entry = appCache.CreateEntry(CountriesKey);

        entry.Value = countries;

        return countries!;
    }

    public async Task<Country?> GetByCodeAsync(Cca2 cca2, CancellationToken cancellationToken)
    {
        string countryKey = $"country:{cca2}";

        if (appCache.TryGetValue(countryKey, out Country? cachedCountry))
            return cachedCountry;

        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.Countries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return null;

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken)
                                                     .ConfigureAwait(false);

        using ICacheEntry entry = appCache.CreateEntry(countryKey);

        Country? country = Array.Find(countries!, c => c.Cca2 == cca2);
        entry.Value = country;

        return country;
    }
}
