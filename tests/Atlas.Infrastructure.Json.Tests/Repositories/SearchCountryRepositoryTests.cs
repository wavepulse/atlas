// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Atlas.Infrastructure.Json.Caching;
using MockHttp;
using System.Net;
using System.Text.Json;

namespace Atlas.Infrastructure.Json.Repositories;

public sealed class SearchCountryRepositoryTests : IDisposable
{
    private const string EndpointUrl = $"{DataJsonPaths.BaseDirectory}/{DataJsonPaths.SearchCountries}";

    private readonly MockHttpHandler _handler = new();
    private readonly HttpClient _client;
    private readonly IAppCache _appCache = Substitute.For<IAppCache>();

    private readonly SearchCountryRepository _repository;

    public SearchCountryRepositoryTests()
    {
        _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        _repository = new SearchCountryRepository(_client, _appCache);
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

        _ = await _repository.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Once);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnEmptyWhenStatusIsNot200()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.NotFound));

        IEnumerable<SearchCountry> countries = await _repository.GetAllAsync(CancellationToken.None);

        countries.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnTheData()
    {
        SearchCountry country = CreateSearchCountry();
        string json = JsonSerializer.Serialize([country], CountryJsonContext.Default.SearchCountryArray);

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(json));

        IEnumerable<SearchCountry> countries = await _repository.GetAllAsync(CancellationToken.None);

        countries.Should().Contain(c => c.Cca2 == country.Cca2);
    }

    [Fact]
    public async Task GetAllAsyncShouldCreateTheEntry()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        _ = await _repository.GetAllAsync(CancellationToken.None);

        _appCache.Received(1).CreateEntry("country:search");
    }

    [Fact]
    public async Task GetAllAsyncShouldGetWhenEntryExists()
    {
        _appCache.TryGetValue<SearchCountry[]>("country:search", out _).Returns(returnThis: true);

        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        _ = await _repository.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Never);
        _appCache.Received(1).TryGetValue<SearchCountry[]>("country:search", out _);
    }

    private static SearchCountry CreateSearchCountry() => new()
    {
        Cca2 = "CA",
        Translations = [new Translation("fra", "Canada")]
    };
}
