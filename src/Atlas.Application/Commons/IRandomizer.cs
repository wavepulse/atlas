// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Commons;

internal interface IRandomizer
{
    T Randomize<T>(IEnumerable<T> source);
}
