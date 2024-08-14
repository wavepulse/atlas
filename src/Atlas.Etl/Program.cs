// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Configurations;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.ConfigureLoggings();
builder.ConfigureHttpClients();

await builder.Build().RunAsync().ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program;
