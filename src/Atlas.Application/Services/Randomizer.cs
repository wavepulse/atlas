// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application.Services;

[ExcludeFromCodeCoverage]
internal sealed class Randomizer : IRandomizer
{
    public T Randomize<T>(ReadOnlySpan<T> items)
    {
        int randomizedIndex = Random.Shared.Next(items.Length);

        return items[randomizedIndex];
    }
}
