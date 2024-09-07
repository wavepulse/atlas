// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Etl.Countries.Dto;
using Atlas.Etl.Countries.Endpoints;
using Atlas.Etl.Countries.Mappers;
using Atlas.Etl.Countries.Settings;
using Atlas.Etl.Json;
using Atlas.Infrastructure.Json;
using Microsoft.Extensions.Logging;

namespace Atlas.Etl.Countries;

internal sealed partial class CountryMigration(ICountryEndpoint endpoint, IJsonFileWriter writer, ILogger<CountryMigration> logger, CountryFilterSettings settings) : IMigration
{
    public string Name { get; } = "Countries";

    public async Task MigrateAsync(string path, CancellationToken cancellationToken)
    {
        CountryDto[] dto = await endpoint.GetAllAsync(cancellationToken).ConfigureAwait(false);

        Country[] countries = dto.AsDomain(settings.Languages);

        Country[] acceptedCountries = countries.ExceptBy(settings.ExcludedCountries, x => x.Cca2, StringComparer.OrdinalIgnoreCase).ToArray();
        Country[] excludedCountries = countries.Except(acceptedCountries).ToArray();
        SearchCountry[] searchCountries = acceptedCountries.AsSearchCountries(settings.ExcludedCountries);

        MigratingCountries(DataJsonPaths.Countries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.Countries}", acceptedCountries, CountryJsonContext.Default.CountryArray, cancellationToken).ConfigureAwait(false);

        MigratingExcludedCountries(DataJsonPaths.ExcludedCountries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.ExcludedCountries}", excludedCountries, CountryJsonContext.Default.CountryArray, cancellationToken).ConfigureAwait(false);

        MigratingCountriesForSearch(DataJsonPaths.SearchCountries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.SearchCountries}", searchCountries, CountryJsonContext.Default.SearchCountryArray, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(LogLevel.Information, "Migrating countries to {jsonFile}")]
    private partial void MigratingCountries(string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating excluded countries to {jsonFile}")]
    private partial void MigratingExcludedCountries(string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating countries for search to {jsonFile}")]
    private partial void MigratingCountriesForSearch(string jsonFile);
}
