// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Services;

internal interface IDateHash
{
    uint Hash(DateOnly currentDate);
}
