// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json;
using System.Diagnostics.CodeAnalysis;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

namespace Prometheus.Patch;

[ExcludeFromCodeCoverage]
internal sealed class CountryPatch : ICountryPatch
{
    public void ApplyTo(ReadOnlySpan<CountryDto> countries)
    {
        Dictionary<string, Operation<CountryDto>[]> patches = GetCountryPatches();

        JsonPatchDocument patch = new([], CountryDtoJsonContext.Default.Options);

        foreach (CountryDto country in countries)
        {
            if (patches.TryGetValue(country.Cca2, out Operation<CountryDto>[]? operations))
            {
                patch.Operations.AddRange(operations);

                patch.ApplyTo(country);

                patch.Operations.Clear();
            }
        }
    }

    private static Dictionary<string, Operation<CountryDto>[]> GetCountryPatches() => new(StringComparer.OrdinalIgnoreCase)
    {
        ["cc"] = [new Operation<CountryDto>("replace", "/coordinate/latitude", from: null, -12.1642)],
        ["hm"] = [new Operation<CountryDto>("replace", "/coordinate/latitude", from: null, -53.0818)],
        ["pf"] =
        [
            new Operation<CountryDto>("replace", "/coordinate/latitude", from: null, -17.6797),
            new Operation<CountryDto>("replace", "/coordinate/longitude", from: null, -149.4068),
        ],
        ["mf"] = [new Operation<CountryDto>("replace", "/coordinate/longitude", from: null, -63.0501)],
        ["fj"] = [new Operation<CountryDto>("replace", "/coordinate/latitude", from: null, -17.7134)],
    };
}
