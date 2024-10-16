﻿// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Caching;
using Atlas.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.Section))
                .AddSingleton<IValidateOptions<CacheOptions>, CacheOptions.Validator>()
                .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheOptions>>().Value)
                .AddOptionsWithValidateOnStart<CacheOptions>();

        services.AddMemoryCache()
                .AddScoped<ICache, Cache>();
    }
}
