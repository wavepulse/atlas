// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Caching;

internal interface ICache
{
    void Save<TItem>(string key, TItem value);

    bool TryGet<TItem>(string key, [NotNullWhen(true)] out TItem? value);
}
