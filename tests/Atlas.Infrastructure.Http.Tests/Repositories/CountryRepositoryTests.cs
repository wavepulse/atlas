// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Infrastructure.Http.Caching;
using Atlas.Infrastructure.Json;
using MockHttp;
using System.Net;
using System.Text.Json;

namespace Atlas.Infrastructure.Http.Repositories;

public sealed class CountryRepositoryTests : IDisposable
{
    private const string EndpointUrl = $"{DataJsonPaths.BaseDirectory}/{DataJsonPaths.Countries}";

    private readonly MockHttpHandler _handler = new();
    private readonly HttpClient _client;
    private readonly IAppCache _appCache = Substitute.For<IAppCache>();

    private readonly CountryRepository _repository;

    public CountryRepositoryTests()
    {
        _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        _repository = new CountryRepository(_client, _appCache);
    }

    public void Dispose()
    {
        _handler.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task GetAllCodesAsyncShouldCallTheEndpointToGetData()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Once);
    }

    [Fact]
    public async Task GetAllCodesAsyncShouldReturnEmptyWhenStatusIsNot200()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.NotFound));

        Country[] countries = await _repository.GetAllAsync(CancellationToken.None);

        countries.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCodesAsyncShouldReturnTheData()
    {
        Country country = CreateCountry();
        string json = JsonSerializer.Serialize([country], CountryJsonContext.Default.CountryArray);

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(json));

        Country[] countries = await _repository.GetAllAsync(CancellationToken.None);

        countries.Should().Contain(c => c.Cca2 == country.Cca2);
    }

    [Fact]
    public async Task GetAllCodesAsyncShouldCreateTheEntry()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetAllAsync(CancellationToken.None);

        _appCache.Received(1).CreateEntry("country:codes");
    }

    [Fact]
    public async Task GetAllCodesAsyncShouldGetWhenEntryExists()
    {
        _appCache.TryGetValue<Country[]>("country:codes", out _).Returns(returnThis: true);

        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Never);
        _appCache.Received(1).TryGetValue<Country[]>("country:codes", out _);
    }

    [Fact]
    public async Task GetByCodeAsyncShouldCallTheEndpointToGetData()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetByCodeAsync(string.Empty, CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Once);
    }

    [Fact]
    public async Task GetByCodeAsyncShouldReturnEmptyWhenStatusIsNot200()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.NotFound));

        Country? country = await _repository.GetByCodeAsync(string.Empty, CancellationToken.None);

        country.Should().BeNull();
    }

    [Fact]
    public async Task GetByCodeAsyncShouldReturnTheCountryBasedOnTheCode()
    {
        Country country = CreateCountry();
        string json = JsonSerializer.Serialize([country], CountryJsonContext.Default.CountryArray);

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(json));

        Country? foundCountry = await _repository.GetByCodeAsync(country.Cca2, CancellationToken.None);

        foundCountry!.Should().BeEquivalentTo(country);
    }

    [Fact]
    public async Task GetByCodeAsyncShouldCreateTheEntry()
    {
        const string cca2 = "CA";
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetByCodeAsync(cca2, CancellationToken.None);

        _appCache.Received(1).CreateEntry($"country:{cca2}");
    }

    [Fact]
    public async Task GetByCodeAsyncShouldGetWhenEntryExists()
    {
        const string cca2 = "CA";

        _appCache.TryGetValue<Country>($"country:{cca2}", out _).Returns(returnThis: true);

        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _repository.GetByCodeAsync(cca2, CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Never);
        _appCache.Received(1).TryGetValue<Country>($"country:{cca2}", out _);
    }

    private static Country CreateCountry() => new()
    {
        Cca2 = "CA",
        Capitals = [new Capital("Ottawa", new Coordinate(0, 0))],
        Area = new Area(1),
        Population = 1,
        Translations = [new Translation(Language.English, "Canada")],
        Borders = ["US"],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(0, 0),
        FlagSvgUri = new Uri("https://www.countryflags.io/ca/flat/64.svg"),
        MapUri = new Uri("https://www.google.com/maps/place/Canada")
    };
}
