// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Services;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries.Queries;

public sealed class GetDailyCountryTests
{
    private readonly Country[] _countries = [CreateCanada()];
    private readonly DateOnly _date = new(2024, 11, 06);

    private readonly IDateHash _dateHash = Substitute.For<IDateHash>();
    private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();

    private readonly GetDailyCountry.Query _query = new();
    private readonly GetDailyCountry.Handler _handler;

    public GetDailyCountryTests()
    {
        _timeService.CurrentDate.Returns(_date);

        _countryRepository.GetAllAsync(CancellationToken.None).Returns(_countries);

        _handler = new GetDailyCountry.Handler(_dateHash, _countryRepository, _timeService);
    }

    [Fact]
    public async Task HandleShouldGetAllCountries()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _countryRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnTheCountryOfTheDay()
    {
        const uint hash = 0;

        _dateHash.Hash(_date).Returns(hash);

        RandomizedCountryResponse country = await _handler.Handle(_query, CancellationToken.None);

        country.Cca2.Should().Be(_countries[0].Cca2);
    }

    [Fact]
    public async Task HandleShouldCacheTheCountryOfTheDay()
    {
        const uint hash = 0;

        _dateHash.Hash(_date).Returns(hash);

        await _handler.Handle(_query, CancellationToken.None);

        _countryRepository.Received(1).Cache(Arg.Is<Country>(c => c.Cca2 == _countries[0].Cca2));
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = ["USA"],
        Capitals = [new Capital("Ottawa", new Coordinate(42, 42))],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, 95),
        Population = 38008005,
        Translations = [new Translation(Language.English, "Canada")],
        IsExcluded = false,
        Resources = new CountryResources()
        {
            Flag = new Image(new Uri("https://www.countryflags.io/ca/flat/64.png"), "image/png"),
            Map = new Uri("https://www.google.com/maps/place/Canada")
        }
    };
}
