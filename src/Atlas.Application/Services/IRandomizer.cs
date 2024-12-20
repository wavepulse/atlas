// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Services;

internal interface IRandomizer
{
    T Randomize<T>(ReadOnlySpan<T> items);

    void Shuffle<T>(Span<T> items);
}
