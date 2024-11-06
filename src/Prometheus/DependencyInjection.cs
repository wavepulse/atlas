// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Files;
using Prometheus.Json;
using Prometheus.Options;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddEtlApplication(this IHostApplicationBuilder builder)
        => builder.Services.AddHostedService<EtlApplication>();

    internal static void AddJsonServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IJsonFile, JsonFile>()
                        .AddTransient<IJsonSerializer, JsonSerializer>()
                        .AddTransient<IJsonFileWriter, JsonFileWriter>();
    }

    internal static void AddFileServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IDirectory, Files.Directory>()
                        .AddTransient<IDataDirectory, DataDirectory>();

        builder.Services.Configure<PathOptions>(builder.Configuration)
                        .AddSingleton<IValidateOptions<PathOptions>, PathOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<PathOptions>>().Value)
                        .AddOptionsWithValidateOnStart<PathOptions>();
    }
}
