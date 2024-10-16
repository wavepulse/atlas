// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Countries.Sources;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Countries;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddCountries(this IServiceCollection services, Action<HttpClient> configure)
    {
        services.ConfigureHttpClientDefaults(c => c.ConfigureHttpClient(configure));

        services.AddHttpClient<IDataSource<Country>, CountryDataSource>();
        services.AddHttpClient<IDataSource<CountryLookup>, CountryLookupDataSource>();
    }
}
