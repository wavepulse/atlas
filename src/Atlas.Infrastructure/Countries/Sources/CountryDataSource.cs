// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Atlas.Infrastructure.Countries.Sources;

[ExcludeFromCodeCoverage]
internal sealed class CountryDataSource(HttpClient client) : IDataSource<Country>
{
    public async Task<Country[]> QueryAllAsync(CancellationToken cancellationToken)
    {
        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.Countries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken)
                                                     .ConfigureAwait(false);

        return countries!;
    }
}
