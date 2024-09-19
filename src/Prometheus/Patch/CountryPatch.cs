// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json;
using Prometheus.Patch.Settings;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

namespace Prometheus.Patch;

internal sealed class CountryPatch(CountryPatchSettings settings) : ICountryPatch
{
    private readonly Dictionary<string, Operation<CountryDto>[]> _patches = new(settings.Patches, StringComparer.OrdinalIgnoreCase);

    public void ApplyTo(ReadOnlySpan<CountryDto> countries)
    {
        JsonPatchDocument patch = new([], CountryDtoJsonContext.Default.Options);

        foreach (CountryDto country in countries)
        {
            if (_patches.TryGetValue(country.Cca2, out Operation<CountryDto>[]? operations))
            {
                patch.Operations.AddRange(operations);

                patch.ApplyTo(country);

                patch.Operations.Clear();
            }
        }
    }
}
