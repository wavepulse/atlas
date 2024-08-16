// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Atlas.Etl.Json;

internal sealed class JsonFileWriter(IJsonFile jsonFile, IJsonSerializer jsonSerializer) : IJsonFileWriter
{
    public async Task WriteToAsync<T>(string path, T value, JsonTypeInfo<T> metadata, CancellationToken cancellationToken)
    {
        Stream stream = jsonFile.OpenWrite(path);

        await using (stream.ConfigureAwait(false))
        {
            await jsonSerializer.SerializeAsync(stream, value, metadata, cancellationToken).ConfigureAwait(false);
        }
    }
}
