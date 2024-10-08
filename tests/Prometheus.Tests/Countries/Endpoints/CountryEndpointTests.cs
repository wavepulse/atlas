// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Logging;
using MockHttp;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Options;
using Prometheus.Fixtures;
using System.Net;

namespace Prometheus.Countries.Endpoints;

public sealed class CountryEndpointTests : IClassFixture<CountriesJson>, IDisposable
{
    private const string EndpointUrl = "https://test.countries.com";

    private readonly MockHttpHandler _handler = new();
    private readonly HttpClient _client;
    private readonly CountriesJson _json;

    private readonly CountryEndpoint _endpoint;

    public CountryEndpointTests(CountriesJson json)
    {
        _json = json;

        CountryEndpointOptions options = new() { Endpoint = EndpointUrl };
        ILogger<CountryEndpoint> logger = Substitute.For<ILogger<CountryEndpoint>>();

        _client = new HttpClient(_handler);

        _endpoint = new CountryEndpoint(_client, logger, options);
    }

    public void Dispose()
    {
        _handler.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task GetAllAsyncShouldCallTheEndpointToGetData()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _endpoint.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Once);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnEmptyWhenStatusIsNot200()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.NotFound));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        countries.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnCountries()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(_json.Canada));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        CountryDto canada = countries[0];

        canada.Cca2.Should().Be("CA");
        canada.Name.Common.Should().Be("Canada");
        canada.Capitals.Should().BeEquivalentTo("Ottawa");
        canada.Region.Should().Be(RegionDto.Americas);
        canada.SubRegion.Should().Be(SubRegionDto.NorthAmerica);
        canada.Translations.Should().Contain(t => t == new TranslationDto("fra", "Canada"));
        canada.Coordinate.Should().Be(new CoordinateDto(60.0, -95.0));
        canada.Borders.Should().BeEquivalentTo("USA");
        canada.Area.Should().Be(9984670);
        canada.Population.Should().Be(38005238);
        canada.CapitalInfo.Coordinate.Should().Be(new CoordinateDto(45.42, -75.7));
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnAllPossibleValuesFromACountryWhichMissingSomeFields()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(_json.Antarctica));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        CountryDto antarctica = countries[0];

        antarctica.Cca2.Should().Be("AQ");
        antarctica.Name.Common.Should().Be("Antarctica");
        antarctica.Capitals.Should().BeNull();
        antarctica.Region.Should().Be(RegionDto.Antarctic);
        antarctica.SubRegion.Should().BeNull();
        antarctica.Translations.Should().Contain(t => t == new TranslationDto("fra", "Antarctique"));
        antarctica.Coordinate.Should().Be(new CoordinateDto(-90, 0));
        antarctica.Borders.Should().BeNull();
        antarctica.Area.Should().Be(14000000);
        antarctica.Population.Should().Be(1000);
        antarctica.CapitalInfo.Coordinate.Should().BeNull();
    }
}
