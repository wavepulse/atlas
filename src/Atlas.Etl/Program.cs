// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl;
using Atlas.Etl.Configurations;
using Atlas.Etl.Countries;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.ConfigureLoggings();
builder.ConfigureHttpClients();

builder.AddJsonServices();
builder.AddFileServices();

builder.AddCountries();
builder.AddEtlApplication();

await builder.Build().RunAsync().ConfigureAwait(false);
