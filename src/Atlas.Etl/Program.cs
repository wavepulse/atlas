// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl;
using Atlas.Etl.Configurations;
using Atlas.Etl.Countries;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.ConfigureLoggings();
builder.ConfigureHttpClients();

builder.AddJsonServices();

builder.AddCountries();

await builder.Build().RunAsync().ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program;
