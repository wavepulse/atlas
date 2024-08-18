// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json.Caching;
using Atlas.Infrastructure.Json.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Atlas.Infrastructure.Json;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.Section))
                    .AddSingleton<IValidateOptions<CacheSettings>, CacheSettings.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheSettings>>().Value)
                    .AddOptionsWithValidateOnStart<CacheSettings>();

        _ = services.AddMemoryCache()
                    .AddScoped<IAppCache, AppCache>();
    }
}
