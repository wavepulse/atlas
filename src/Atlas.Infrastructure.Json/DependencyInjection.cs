// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Infrastructure.Json.Caching;
using Atlas.Infrastructure.Json.Repositories;
using Atlas.Infrastructure.Json.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Json;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<HttpClient> configure)
    {
        _ = services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.Section))
                    .AddSingleton<IValidateOptions<CacheSettings>, CacheSettings.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheSettings>>().Value)
                    .AddOptionsWithValidateOnStart<CacheSettings>();

        _ = services.ConfigureHttpClientDefaults(c => c.ConfigureHttpClient(configure));

        _ = services.AddHttpClient<ISearchCountryRepository, SearchCountryRepository>();
        _ = services.AddHttpClient<ICountryRepository, CountryRepository>();

        _ = services.AddMemoryCache()
                    .AddScoped<IAppCache, AppCache>();
    }
}
