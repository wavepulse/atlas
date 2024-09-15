// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Prometheus.Configurations;

[ExcludeFromCodeCoverage]
internal static class LoggingConfigurations
{
    internal static void ConfigureLoggings(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = true);
        _ = builder.Logging.ClearProviders();
        _ = builder.Logging.AddConsole(options => options.FormatterName = "custom");
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ConsoleFormatter, CustomConsoleFormatter>());
    }
}

file sealed class CustomConsoleFormatter : ConsoleFormatter
{
    private const string DefaultForegroundColor = "\e[39m\e[22m";
    private const string DefaultBackgroundColor = "\e[49m";

    public CustomConsoleFormatter() : base("custom")
    {
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        string message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        WriteInternal(textWriter, message, logEntry.LogLevel);
    }

    private static void WriteInternal(TextWriter textWriter, string message, LogLevel logLevel)
    {
        textWriter.Write(DateTimeOffset.Now.ToString("[HH:mm:ss] "));
        WriteLogLevel(textWriter, logLevel);
        WriteMessage(textWriter, message);

        textWriter.Write(Environment.NewLine);
    }

    private static void WriteMessage(TextWriter textWriter, string message)
    {
        textWriter.Write(' ');
        WriteReplacing(textWriter, Environment.NewLine, string.Empty, message);

        static void WriteReplacing(TextWriter writer, string oldValue, string newValue, string message)
        {
            string newMessage = message.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
            writer.Write(newMessage);
        }
    }

    private static void WriteLogLevel(TextWriter textWriter, LogLevel logLevel)
    {
        ConsoleColors colors = GetLogLevelColors(logLevel);

        if (colors.Background.HasValue)
            textWriter.Write(GetColorEscapeCode(colors.Background.Value));

        if (colors.Foreground.HasValue)
            textWriter.Write(GetColorEscapeCode(colors.Foreground.Value));

        textWriter.Write($"{GetLogLevel(logLevel)}:");

        if (colors.Foreground.HasValue)
            textWriter.Write(DefaultForegroundColor);

        if (colors.Background.HasValue)
            textWriter.Write(DefaultBackgroundColor);
    }

    private static string GetLogLevel(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "trace",
        LogLevel.Debug => "debug",
        LogLevel.Information => "info",
        LogLevel.Warning => "warn",
        LogLevel.Error => "error",
        LogLevel.Critical => "critical",
        LogLevel.None => "none",
        _ => "unknown",
    };

    private static ConsoleColors GetLogLevelColors(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
        LogLevel.Debug => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
        LogLevel.Information => new ConsoleColors(ConsoleColor.Cyan, ConsoleColor.Black),
        LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black),
        LogLevel.Error => new ConsoleColors(ConsoleColor.Red, ConsoleColor.Black),
        LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.Red),
        LogLevel.None => new ConsoleColors(ConsoleColor.White, ConsoleColor.Black),
        _ => new ConsoleColors(foreground: null, background: null),
    };

    private static string GetColorEscapeCode(ConsoleColor color) => color switch
    {
        ConsoleColor.Black => "\e[30m",
        ConsoleColor.DarkRed => "\e[31m",
        ConsoleColor.DarkGreen => "\e[32m",
        ConsoleColor.DarkYellow => "\e[33m",
        ConsoleColor.DarkBlue => "\e[34m",
        ConsoleColor.DarkMagenta => "\e[35m",
        ConsoleColor.DarkCyan => "\e[36m",
        ConsoleColor.Gray => "\e[37m",
        ConsoleColor.Red => "\e[1m\e[31m",
        ConsoleColor.Green => "\e[1m\e[32m",
        ConsoleColor.Yellow => "\e[1m\e[33m",
        ConsoleColor.Blue => "\e[1m\e[34m",
        ConsoleColor.Magenta => "\e[1m\e[35m",
        ConsoleColor.Cyan => "\e[1m\e[36m",
        ConsoleColor.White => "\e[1m\e[37m",
        ConsoleColor.DarkGray => "\e[37m",
        _ => DefaultForegroundColor
    };

    [StructLayout(LayoutKind.Auto)]
    private readonly struct ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
    {
        public ConsoleColor? Foreground { get; } = foreground;

        public ConsoleColor? Background { get; } = background;
    }
}
