// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Countries.Repositories;
using Atlas.Infrastructure.Countries.Sources;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddCountries(this IServiceCollection services, Action<HttpClient> configure)
    {
        services.AddHttpClient<IDataSource<Country>, CountryDataSource>(configure);
        services.AddHttpClient<IDataSource<CountryLookup>, CountryLookupDataSource>(configure);

        services.AddTransient<ICountryLookupRepository, CountryLookupRepository>();
        services.AddTransient<ICountryRepository, CountryRepository>();
    }
}
