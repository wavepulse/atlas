// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Patch.Settings;
using SystemTextJsonPatch.Operations;

namespace Prometheus.Patch;

public sealed class CountryPatchTests
{
    private const double Latitude = -17.81;

    private readonly CountryPatchSettings _settings = new()
    {
        Patches = new Dictionary<string, Operation<CountryDto>[]>()
        {
            ["fj"] = [new("replace", "coordinate/latitude", from: null, Latitude)]
        }
    };

    private readonly CountryPatch _patch;

    public CountryPatchTests()
    {
        _patch = new CountryPatch(_settings);
    }

    [Fact]
    public void ApplyToShouldPatchTheCountry()
    {
        CountryDto fiji = CreateCountry("fj");

        _patch.ApplyTo([fiji]);

        fiji.Coordinate.Latitude.Should().Be(Latitude);
    }

    [Fact]
    public void ApplyToShouldNotPatchTheCountryIsNotInSettings()
    {
        CountryDto canada = CreateCountry("ca");

        _patch.ApplyTo([canada]);

        canada.Coordinate.Latitude.Should().NotBe(Latitude);
    }

    private static CountryDto CreateCountry(string cca2) => new()
    {
        Cca2 = cca2,
        Coordinate = new CoordinateDto(42, 42),
        Area = 18274,
        CapitalInfo = new CapitalInfoDto() { Coordinate = new(42, 42) },
        Flags = new FlagsDto(new Uri("https://restcountries.com/data/fji.svg")),
        Maps = new MapsDto(new Uri("https://goo.gl/maps/1")),
        Name = new NameDto("country"),
        Population = 867000,
        Region = RegionDto.Oceania
    };
}
