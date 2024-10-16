// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json;
using System.Net.Http.Json;

namespace Atlas.Infrastructure.Countries.Sources;

internal sealed class CountryLookupDataSource(HttpClient client) : IDataSource<CountryLookup>
{
    public async Task<CountryLookup[]> QueryAllAsync(CancellationToken cancellationToken)
    {
        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.SearchCountries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        CountryLookup[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryLookupArray, cancellationToken)
                                                           .ConfigureAwait(false);
        return countries!;
    }
}
