// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Json.Caching;

internal interface IAppCache
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory);
}
