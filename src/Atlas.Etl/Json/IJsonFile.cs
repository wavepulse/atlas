// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Etl.Json;

internal interface IJsonFile
{
    Stream OpenWrite(string path);
}
