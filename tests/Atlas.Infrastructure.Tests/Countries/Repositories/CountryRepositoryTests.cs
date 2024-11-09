// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;
using Atlas.Infrastructure.Caching;
using Atlas.Infrastructure.Countries.Sources;

namespace Atlas.Infrastructure.Countries.Repositories;

public sealed class CountryRepositoryTests
{
    private const string ExpectedAllCountriesKey = "countries";

    private readonly IDataSource<Country> _dataSource = Substitute.For<IDataSource<Country>>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly CountryRepository _repository;

    public CountryRepositoryTests()
    {
        _repository = new CountryRepository(_dataSource, _cache);
    }

    [Fact]
    public void CacheShouldCacheCountry()
    {
        Country country = CreateCountry("CA");
        string expectedKey = $"{ExpectedAllCountriesKey}:{country.Cca2}";

        _repository.Cache(country);

        _cache.Received(1).Save(expectedKey, country);
    }

    [Fact]
    public async Task GetAllAsyncShouldGetAllCountries()
    {
        await _repository.GetAllAsync(CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetAllAsyncShouldCacheAllCountries()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(CancellationToken.None).Returns([country]);

        await _repository.GetAllAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedAllCountriesKey, Arg.Is<Country[]>(c => c.Contains(country)));
    }

    [Fact]
    public async Task GetAllAsyncShouldNotRetrieveFromDataSourceIsAllCountriesAreCached()
    {
        _cache.TryGet<Country[]>(ExpectedAllCountriesKey, out _).Returns(returnThis: true);

        await _repository.GetAllAsync(CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetAsyncShouldGetAllCountries()
    {
        await _repository.GetAsync(new Cca2("CA"), CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetAsyncShouldGetCountryByCca2()
    {
        Country canada = CreateCountry("CA");
        Country usa = CreateCountry("US");

        _dataSource.QueryAllAsync(CancellationToken.None).Returns([canada, usa]);

        Country? foundCountry = await _repository.GetAsync(canada.Cca2, CancellationToken.None);

        foundCountry.Should().Be(canada);
    }

    [Fact]
    public async Task GetAsyncShouldReturnNullWhenDoesNotFoundTheCountry()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(CancellationToken.None).Returns([country]);

        Country? foundCountry = await _repository.GetAsync(new Cca2("US"), CancellationToken.None);

        foundCountry.Should().BeNull();
    }

    [Fact]
    public async Task GetAsyncShouldCacheTheCountry()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(CancellationToken.None).Returns([country]);

        string expectedKey = $"{ExpectedAllCountriesKey}:{country.Cca2}";

        await _repository.GetAsync(country.Cca2, CancellationToken.None);

        _cache.Received(1).Save(expectedKey, Arg.Is<Country>(c => c.Cca2 == country.Cca2));
    }

    [Fact]
    public async Task GetAsyncShouldNotRetrieveFromDataSourceIsCountryCached()
    {
        Cca2 cca2 = new("CA");
        string expectedKey = $"{ExpectedAllCountriesKey}:{cca2}";

        _cache.TryGet<Country>(expectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(cca2, CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(CancellationToken.None);
    }

    private static Country CreateCountry(string cca2) => new()
    {
        Cca2 = new Cca2(cca2),
        Capitals = [new Capital("Ottawa", new Coordinate(0, 0))],
        Area = new Area(1),
        Population = 1,
        Translations = [new Translation(Language.English, "Canada")],
        Borders = ["US"],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(0, 0),
        IsExcluded = false,
        Resources = new CountryResources()
        {
            Flag = new Image(new Uri("https://www.countryflags.io/ca/flat/64.svg"), "image/svg+xml"),
            Map = new Uri("https://www.google.com/maps/place/Canada")
        }
    };
}
