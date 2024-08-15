// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;
using Atlas.Etl.Countries.Json;
using Atlas.Etl.Countries.Settings;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Atlas.Etl.Countries.Endpoints;

internal sealed partial class CountryEndpoint(HttpClient client, ILogger<CountryEndpoint> logger, CountryEndpointSettings settings) : ICountryEndpoint
{
    public async Task<CountryDto[]> GetAllAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await client.GetAsync(settings.Endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                                                         .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            FailedToFetchCountries(settings.Endpoint);
            return [];
        }

        CountryDto[]? countries = await response.Content.ReadFromJsonAsync(CountryDtoJsonContext.Default.CountryDtoArray, cancellationToken)
                                                        .ConfigureAwait(false);

        FetchedCountries(settings.Endpoint);

        return countries!;
    }

    [LoggerMessage(LogLevel.Warning, "Failed to fetch countries from {endpoint}")]
    private partial void FailedToFetchCountries(string endpoint);

    [LoggerMessage(LogLevel.Information, "Fetched countries from {endpoint}")]
    private partial void FetchedCountries(string endpoint);
}
