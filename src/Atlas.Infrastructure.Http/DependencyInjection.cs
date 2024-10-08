// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Infrastructure.Http.Caching;
using Atlas.Infrastructure.Http.Options;
using Atlas.Infrastructure.Http.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Http;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddHttpInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<HttpClient> configure)
    {
        _ = services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.Section))
                    .AddSingleton<IValidateOptions<CacheOptions>, CacheOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheOptions>>().Value)
                    .AddOptionsWithValidateOnStart<CacheOptions>();

        _ = services.ConfigureHttpClientDefaults(c => c.ConfigureHttpClient(configure));

        _ = services.AddHttpClient<ISearchCountryRepository, SearchCountryRepository>();
        _ = services.AddHttpClient<ICountryRepository, CountryRepository>();

        _ = services.AddMemoryCache()
                    .AddScoped<IAppCache, AppCache>();
    }
}
