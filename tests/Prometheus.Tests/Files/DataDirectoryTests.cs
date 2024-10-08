// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Infrastructure.Json;
using NSubstitute.ReturnsExtensions;
using Prometheus.Options;

namespace Prometheus.Files;

public sealed class DataDirectoryTests
{
    private readonly string _expectedPath;
    private readonly IDirectory _directory = Substitute.For<IDirectory>();
    private readonly PathOptions _options = new()
    {
        Root = "root",
        Output = "output"
    };

    private readonly DataDirectory _dataDirectory;

    public DataDirectoryTests()
    {
        _expectedPath = Path.Combine(_options.Root, _options.Output, DataJsonPaths.BaseDirectory);

        _dataDirectory = new DataDirectory(_directory, _options);
    }

    [Fact]
    public void CreateShouldGetTheRootPath()
    {
        _ = _dataDirectory.Create();

        _directory.Received(1).GetRootPath(_options.Root);
    }

    [Fact]
    public void CreateShouldCreateDataFolder()
    {
        _directory.GetRootPath(_options.Root).Returns(_options.Root);

        _ = _dataDirectory.Create();

        _directory.Received(1).Create(_expectedPath);
    }

    [Fact]
    public void CreateShouldReturnThePath()
    {
        _directory.GetRootPath(_options.Root).Returns(_options.Root);
        _directory.Create(_expectedPath).Returns(_expectedPath);

        string? path = _dataDirectory.Create();

        path.Should().Be(_expectedPath);
    }

    [Fact]
    public void CreateShouldReturnNullWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_options.Root).ReturnsNull();

        string? path = _dataDirectory.Create();

        path.Should().BeNull();
    }

    [Fact]
    public void CreateShouldNotCreateTheDirectoryWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_options.Root).ReturnsNull();

        _ = _dataDirectory.Create();

        _directory.DidNotReceive().Create(_expectedPath);
    }
}
