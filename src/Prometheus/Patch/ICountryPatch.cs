// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;

namespace Prometheus.Patch;

internal interface ICountryPatch
{
    void ApplyTo(ReadOnlySpan<CountryDto> countries);
}
