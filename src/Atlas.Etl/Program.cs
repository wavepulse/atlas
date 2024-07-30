// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

await builder.Build().RunAsync().ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program;
