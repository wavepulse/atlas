// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;
using Prometheus.Files;

namespace Prometheus;

public sealed class EtlApplicationTests
{
    private readonly IDataDirectory _dataDirectory = Substitute.For<IDataDirectory>();
    private readonly IMigration _migration = Substitute.For<IMigration>();
    private readonly IHostApplicationLifetime _lifetime = Substitute.For<IHostApplicationLifetime>();
    private readonly IHostEnvironment _environment = Substitute.For<IHostEnvironment>();

    private readonly EtlApplication _application;

    public EtlApplicationTests()
    {
        ILogger<EtlApplication> logger = Substitute.For<ILogger<EtlApplication>>();

        _application = new EtlApplication(_dataDirectory, [_migration], _lifetime, _environment, logger);
    }

    [Fact]
    public async Task StartAsyncShouldCreateTheDataDirectory()
    {
        await _application.StartAsync(CancellationToken.None);

        _dataDirectory.Received(1).Create();
    }

    [Fact]
    public async Task StartAsyncShouldStopApplicationWhenDataDirectoryIsNotCreated()
    {
        _dataDirectory.Create().ReturnsNull();

        await _application.StartAsync(CancellationToken.None);

        _lifetime.Received(1).StopApplication();
    }

    [Fact]
    public async Task StartAsyncShouldNotMigrateWhenDataDirectoryIsNotCreated()
    {
        _dataDirectory.Create().ReturnsNull();

        await _application.StartAsync(CancellationToken.None);

        await _migration.DidNotReceive().MigrateAsync(Arg.Any<string>(), CancellationToken.None);
    }

    [Fact]
    public async Task StartAsyncShouldMigrateWhenDataDirectoryIsCreated()
    {
        _dataDirectory.Create().Returns("path");

        await _application.StartAsync(CancellationToken.None);

        await _migration.Received(1).MigrateAsync("path", CancellationToken.None);
    }

    [Fact]
    public async Task StartAsyncShouldStopApplicationWhenFinishedMigration()
    {
        _dataDirectory.Create().Returns("path");

        await _application.StartAsync(CancellationToken.None);

        await _migration.Received(1).MigrateAsync("path", CancellationToken.None);
        _lifetime.Received(1).StopApplication();
    }
}
