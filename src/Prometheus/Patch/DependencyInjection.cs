// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Patch.Settings;

namespace Prometheus.Patch;

internal static class DependencyInjection
{
    internal static void AddPatchServices(this IHostApplicationBuilder builder)
    {
        _ = builder.Configuration.AddJsonFile("patch.json", optional: false, reloadOnChange: false);

        _ = builder.Services.Configure<CountryPatchSettings>(builder.Configuration.GetSection(CountryPatchSettings.Section))
                            .AddSingleton<IValidateOptions<CountryPatchSettings>, CountryPatchSettings.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryPatchSettings>>().Value)
                            .AddOptionsWithValidateOnStart<CountryPatchSettings>();

        _ = builder.Services.AddTransient<ICountryPatch, CountryPatch>();
    }
}
