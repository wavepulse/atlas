// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus.Fixtures;

public sealed class CountriesJson
{
    public string Canada { get; } = GetJson("canada.json");

    public string Antarctica { get; } = GetJson("antarctica.json");

    private static string GetJson(string filename)
    {
        const string path = "Fixtures/Data";

        return File.ReadAllText(Path.Combine(path, filename));
    }
}
