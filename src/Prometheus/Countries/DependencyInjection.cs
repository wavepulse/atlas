// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Countries.Endpoints;
using Prometheus.Countries.Options;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddCountries(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.Configure<CountryEndpointOptions>(builder.Configuration.GetSection(CountryEndpointOptions.Section))
                            .AddSingleton<IValidateOptions<CountryEndpointOptions>, CountryEndpointOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryEndpointOptions>>().Value)
                            .AddOptionsWithValidateOnStart<CountryEndpointOptions>();

        _ = builder.Services.Configure<CountryFilterOptions>(builder.Configuration.GetSection(CountryFilterOptions.Section))
                            .AddSingleton<IValidateOptions<CountryFilterOptions>, CountryFilterOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryFilterOptions>>().Value)
                            .AddOptionsWithValidateOnStart<CountryFilterOptions>();

        _ = builder.Services.AddHttpClient<ICountryEndpoint, CountryEndpoint>();
        _ = builder.Services.AddTransient<IMigration, CountryMigration>();
    }
}
