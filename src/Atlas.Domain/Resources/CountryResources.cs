// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Resources;

public sealed record CountryResources
{
    public required Uri Map { get; init; }

    public required Image Flag { get; init; }

    public void Deconstruct(out Uri map, out Image flag)
    {
        map = Map;
        flag = Flag;
    }
}
