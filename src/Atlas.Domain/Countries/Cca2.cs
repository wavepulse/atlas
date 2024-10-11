// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Countries;

public readonly record struct Cca2(string Value)
{
    public static implicit operator string(Cca2 cca2) => cca2.Value;

    public bool Equals(Cca2 other) => Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Value;
}
