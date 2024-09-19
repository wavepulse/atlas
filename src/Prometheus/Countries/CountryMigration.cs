// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json;
using Microsoft.Extensions.Logging;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Endpoints;
using Prometheus.Countries.Mappers;
using Prometheus.Countries.Settings;
using Prometheus.Json;
using Prometheus.Patch;

namespace Prometheus.Countries;

internal sealed partial class CountryMigration(ICountryEndpoint endpoint, IJsonFileWriter writer, ICountryPatch countryPatch, ILogger<CountryMigration> logger, CountryFilterSettings settings) : IMigration
{
    public string Name { get; } = "Countries";

    public async Task MigrateAsync(string path, CancellationToken cancellationToken)
    {
        CountryDto[] dto = await endpoint.GetAllAsync(cancellationToken).ConfigureAwait(false);

        PatchingCountries();
        countryPatch.ApplyTo(dto);

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

    [LoggerMessage(LogLevel.Information, "Patching countries")]
    private partial void PatchingCountries();

    [LoggerMessage(LogLevel.Information, "Migrating countries to {jsonFile}")]
    private partial void MigratingCountries(string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating excluded countries to {jsonFile}")]
    private partial void MigratingExcludedCountries(string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating countries for search to {jsonFile}")]
    private partial void MigratingCountriesForSearch(string jsonFile);
}
