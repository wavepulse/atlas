// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Configurations;

[ExcludeFromCodeCoverage]
internal static class LoggingConfigurations
{
    internal static void ConfigureLoggings(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = true);
        _ = builder.Logging.ClearProviders();

        _ = builder.Logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.TimestampFormat = "[HH:mm:ss] ";
            options.SingleLine = true;
        });
    }
}
