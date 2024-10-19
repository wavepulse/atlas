// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Caching;
using Atlas.Infrastructure.Countries.Sources;

namespace Atlas.Infrastructure.Countries.Repositories;

internal sealed class CountryRepository(IDataSource<Country> dataSource, ICache cache) : ICountryRepository
{
    private const string Key = "countries";

    public void Cache(Country country) => cache.Save(AsKey(country.Cca2), country);

    public async ValueTask<Country[]> GetAllAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out Country[]? cachedCountries))
            return cachedCountries;

        Country[] countries = await dataSource.QueryAllAsync(cancellationToken).ConfigureAwait(false);

        cache.Save(Key, countries);

        return countries;
    }

    public async ValueTask<Country?> GetAsync(Cca2 cca2, CancellationToken cancellationToken)
    {
        string countryKey = AsKey(cca2);

        if (cache.TryGet(countryKey, out Country? cachedCountry))
            return cachedCountry;

        Country[] countries = await GetAllAsync(cancellationToken).ConfigureAwait(false);

        Country? country = Array.Find(countries, c => c.Cca2 == cca2);

        if (country is null)
            return null;

        cache.Save(countryKey, country);

        return country;
    }

    private static string AsKey(Cca2 cca2) => $"{Key}:{cca2}";
}
