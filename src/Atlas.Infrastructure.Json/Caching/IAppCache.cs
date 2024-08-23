// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Json.Caching;

internal interface IAppCache
{
    ICacheEntry CreateEntry(string key);

    bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? value);
}
