// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Patch;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddPatchServices(this IHostApplicationBuilder builder)
        => builder.Services.AddTransient<ICountryPatch, CountryPatch>();
}
