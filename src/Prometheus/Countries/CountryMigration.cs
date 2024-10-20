// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Json;
using Microsoft.Extensions.Logging;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Endpoints;
using Prometheus.Countries.Mappers;
using Prometheus.Countries.Options;
using Prometheus.Json;
using Prometheus.Patch;

namespace Prometheus.Countries;

internal sealed partial class CountryMigration(ICountryEndpoint endpoint, IJsonFileWriter writer, ICountryPatch countryPatch, ILogger<CountryMigration> logger, CountryFilterOptions options) : IMigration
{
    public string Name { get; } = "Countries";

    public async Task MigrateAsync(string path, CancellationToken cancellationToken)
    {
        CountryDto[] dto = await endpoint.GetAllAsync(cancellationToken).ConfigureAwait(false);

        if (dto.Length == 0)
            return;

        PatchingCountries();
        countryPatch.ApplyTo(dto);

        Country[] countries = dto.AsDomain(options.Languages, options.ExcludedCountries);
        CountryLookup[] countryLookups = countries.AsLookups();

        MigratingCountries(countries.Length, DataJsonPaths.Countries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.Countries}", countries, CountryJsonContext.Default.CountryArray, cancellationToken).ConfigureAwait(false);

        MigratingCountriesForSearch(countryLookups.Length, DataJsonPaths.LookupCountries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.LookupCountries}", countryLookups, CountryJsonContext.Default.CountryLookupArray, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(LogLevel.Information, "Patching countries")]
    private partial void PatchingCountries();

    [LoggerMessage(LogLevel.Information, "Migrating {length} country to {jsonFile}")]
    private partial void MigratingCountries(int length, string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating {length} countries for search to {jsonFile}")]
    private partial void MigratingCountriesForSearch(int length, string jsonFile);
}
