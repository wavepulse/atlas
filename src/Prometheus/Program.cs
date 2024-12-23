// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Prometheus;
using Prometheus.Configurations;
using Prometheus.Countries;
using Prometheus.Patch;
using System.Diagnostics.CodeAnalysis;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.ConfigureLoggings();
builder.ConfigureHttpClients();

builder.AddJsonServices();
builder.AddFileServices();
builder.AddPatchServices();

builder.AddCountries();
builder.AddEtlApplication();

await builder.Build().RunAsync().ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program;
