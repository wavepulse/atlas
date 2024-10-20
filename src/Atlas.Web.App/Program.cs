// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application;
using Atlas.Infrastructure;
using Atlas.Web.App;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics.CodeAnalysis;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>(".main-layout");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.ConfigureLoggings();
builder.AddJsRuntime();
builder.AddOptions();
builder.AddFluxor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

await builder.Build().RunAsync().ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program;
