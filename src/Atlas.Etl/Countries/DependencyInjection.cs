// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Endpoints;
using Atlas.Etl.Countries.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Etl.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddCountries(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.Configure<CountryEndpointSettings>(builder.Configuration.GetSection(CountryEndpointSettings.Section))
                            .AddSingleton<IValidateOptions<CountryEndpointSettings>, CountryEndpointSettings.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryEndpointSettings>>().Value)
                            .AddOptionsWithValidateOnStart<CountryEndpointSettings>();

        _ = builder.Services.AddHttpClient<ICountryEndpoint, CountryEndpoint>();
    }
}
