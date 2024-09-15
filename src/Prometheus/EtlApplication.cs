// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus.Files;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Prometheus;

internal sealed partial class EtlApplication(IDataDirectory dataDirectory, IEnumerable<IMigration> migrations, IHostApplicationLifetime lifetime, IHostEnvironment environment, ILogger<EtlApplication> logger) : IHostedService
{
    private readonly Stopwatch _stopwatch = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        DisplayBanner();

        string? path = dataDirectory.Create();

        if (string.IsNullOrEmpty(path))
        {
            FailedToCreateDataDirectory();

            lifetime.StopApplication();
            return;
        }

        DataDirectoryCreated();

        foreach (IMigration migration in migrations)
        {
            MigrationStarted(migration.Name);

            _stopwatch.Start();
            await migration.MigrateAsync(path, cancellationToken).ConfigureAwait(false);

            _stopwatch.Stop();
            MigrationCompleted(migration.Name, _stopwatch.ElapsedMilliseconds);

            _stopwatch.Reset();
        }

        lifetime.StopApplication();
    }

    [ExcludeFromCodeCoverage]
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void DisplayBanner()
    {
        AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();

        string name = assembly.Name!;
        Version version = assembly.Version!;
        string information = $"{name} - {version.Major}.{version.Minor}.{version.Revision} - {environment.EnvironmentName}";

        Console.WriteLine(information);
        Console.WriteLine(new string('=', information.Length));
    }

    [LoggerMessage(LogLevel.Error, "Failed to create the data directory")]
    private partial void FailedToCreateDataDirectory();

    [LoggerMessage(LogLevel.Information, "Data directory created")]
    private partial void DataDirectoryCreated();

    [LoggerMessage(LogLevel.Information, "Starting {migrationName} migration")]
    private partial void MigrationStarted(string migrationName);

    [LoggerMessage(LogLevel.Information, "{migrationName} migration completed in {ElapsedMilliseconds}ms")]
    private partial void MigrationCompleted(string migrationName, long ElapsedMilliseconds);
}
