// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Etl;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddJsonServices(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.AddSingleton<IJsonFile, JsonFile>()
                            .AddSingleton<IJsonSerializer, JsonSerializer>()
                            .AddSingleton<IJsonFileWriter, JsonFileWriter>();
    }
}
