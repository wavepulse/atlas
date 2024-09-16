// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using NSubstitute.ReceivedExtensions;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Prometheus.Json;

public sealed class JsonFileWriterTests
{
    private const string Path = "atlas.json";
    private const int Value = 42;

    private readonly IJsonFile _jsonFile = Substitute.For<IJsonFile>();
    private readonly IJsonSerializer _serializer = Substitute.For<IJsonSerializer>();
    private readonly Stream _stream = Substitute.For<Stream>();
    private readonly JsonTypeInfo<int> _metadata = JsonTypeInfo.CreateJsonTypeInfo<int>(new JsonSerializerOptions());

    private readonly JsonFileWriter _writer;

    public JsonFileWriterTests()
    {
        _jsonFile.OpenWrite(Path).Returns(_stream);

        _writer = new JsonFileWriter(_jsonFile, _serializer);
    }

    [Fact]
    public async Task WriteToAsyncShouldOpenTheJsonFile()
    {
        await _writer.WriteToAsync(Path, Value, _metadata, CancellationToken.None);

        _jsonFile.Received(1).OpenWrite(Path);
    }

    [Fact]
    public async Task WriteToAsyncShouldSerializeTheValue()
    {
        await _writer.WriteToAsync(Path, Value, _metadata, CancellationToken.None);

        await _serializer.Received(1).SerializeAsync(_stream, Value, _metadata, CancellationToken.None);
    }

    [Fact]
    public async Task WriteToAsyncShouldDisposeTheStream()
    {
        await _writer.WriteToAsync(Path, Value, _metadata, CancellationToken.None);

        await _stream.Received(1).DisposeAsync();
    }
}
