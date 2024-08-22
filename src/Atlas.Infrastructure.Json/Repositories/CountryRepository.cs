// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Atlas.Infrastructure.Json.Repositories;

internal sealed class CountryRepository(HttpClient client, IAppCache appCache) : ICountryRepository
{
    private const string CountryCodesKey = "country:codes";

    public async Task<string[]> GetAllCodesAsync(CancellationToken cancellationToken)
    {
        if (appCache.TryGetValue(CountryCodesKey, out string[]? cachedCodes))
            return cachedCodes;

        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.Countries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken)
                                                     .ConfigureAwait(false);

        using ICacheEntry entry = appCache.CreateEntry(CountryCodesKey);

        string[] codes = countries!.Select(c => c.Cca2).ToArray();
        entry.Value = codes;

        return codes;
    }

    public async Task<Country?> GetByCodeAsync(string cca2, CancellationToken cancellationToken)
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

        Country? country = Array.Find(countries, c => c.Cca2.Equals(cca2, StringComparison.OrdinalIgnoreCase));
        entry.Value = country;

        return country;
    }
}
