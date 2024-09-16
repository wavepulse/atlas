// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Files;
using Atlas.Etl.Json;
using Atlas.Etl.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Etl;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddEtlApplication(this IHostApplicationBuilder builder)
        => builder.Services.AddHostedService<EtlApplication>();

    internal static void AddJsonServices(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.AddTransient<IJsonFile, JsonFile>()
                            .AddTransient<IJsonSerializer, JsonSerializer>()
                            .AddTransient<IJsonFileWriter, JsonFileWriter>();
    }

    internal static void AddFileServices(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.AddTransient<IDirectory, Files.Directory>()
                            .AddTransient<IDataDirectory, DataDirectory>();

        _ = builder.Services.Configure<PathSettings>(builder.Configuration)
                            .AddSingleton<IValidateOptions<PathSettings>, PathSettings.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<PathSettings>>().Value)
                            .AddOptionsWithValidateOnStart<PathSettings>();
    }
}
