// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json;
using NSubstitute.ReturnsExtensions;
using Prometheus.Settings;

namespace Prometheus.Files;

public sealed class DataDirectoryTests
{
    private readonly string _expectedPath;
    private readonly IDirectory _directory = Substitute.For<IDirectory>();
    private readonly PathSettings _settings = new()
    {
        Root = "root",
        Output = "output"
    };

    private readonly DataDirectory _dataDirectory;

    public DataDirectoryTests()
    {
        _expectedPath = Path.Combine(_settings.Root, _settings.Output, DataJsonPaths.BaseDirectory);

        _dataDirectory = new DataDirectory(_directory, _settings);
    }

    [Fact]
    public void CreateShouldGetTheRootPath()
    {
        _ = _dataDirectory.Create();

        _directory.Received(1).GetRootPath(_settings.Root);
    }

    [Fact]
    public void CreateShouldCreateDataFolder()
    {
        _directory.GetRootPath(_settings.Root).Returns(_settings.Root);

        _ = _dataDirectory.Create();

        _directory.Received(1).Create(_expectedPath);
    }

    [Fact]
    public void CreateShouldReturnThePath()
    {
        _directory.GetRootPath(_settings.Root).Returns(_settings.Root);
        _directory.Create(_expectedPath).Returns(_expectedPath);

        string? path = _dataDirectory.Create();

        path.Should().Be(_expectedPath);
    }

    [Fact]
    public void CreateShouldReturnNullWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_settings.Root).ReturnsNull();

        string? path = _dataDirectory.Create();

        path.Should().BeNull();
    }

    [Fact]
    public void CreateShouldNotCreateTheDirectoryWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_settings.Root).ReturnsNull();

        _ = _dataDirectory.Create();

        _directory.DidNotReceive().Create(_expectedPath);
    }
}
