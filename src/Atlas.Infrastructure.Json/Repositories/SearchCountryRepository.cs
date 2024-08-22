// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Atlas.Infrastructure.Json.Repositories;

internal sealed class SearchCountryRepository(HttpClient httpClient, IAppCache appCache) : ISearchCountryRepository
{
    private const string Key = "country:search";

    public async Task<SearchCountry[]> GetAllAsync(CancellationToken cancellationToken)
    {
        if (appCache.TryGetValue(Key, out SearchCountry[]? cachedCountries))
            return cachedCountries;

        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.SearchCountries);
        using HttpResponseMessage response = await httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        SearchCountry[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.SearchCountryArray, cancellationToken)
                                                           .ConfigureAwait(false);

        using ICacheEntry entry = appCache.CreateEntry(Key);

        entry.Value = countries!;

        return countries!;
    }
}
