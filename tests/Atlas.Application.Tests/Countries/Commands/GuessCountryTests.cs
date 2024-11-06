// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries.Commands;

public sealed class GuessCountryTests
{
    private readonly Country _canada = CreateCanada();
    private readonly Country _italy = CreateItaly();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GuessCountry.Handler _handler;

    public GuessCountryTests()
    {
        _repository.GetAsync(_canada.Cca2, CancellationToken.None).Returns(_canada);
        _repository.GetAsync(_italy.Cca2, CancellationToken.None).Returns(_italy);

        _handler = new GuessCountry.Handler(_repository);
    }

    [Fact]
    public async Task HandleShouldReturnTheGuessedCountryWhenIsNotSameCountry()
    {
        GuessCountry.Command command = new(_canada.Cca2, _italy.Cca2);

        GuessedCountryResponse guessedCountry = await _handler.Handle(command, CancellationToken.None);

        guessedCountry.Cca2.Should().Be(_canada.Cca2);
        guessedCountry.Name.Should().Be(_canada.Translations.First().Name);
        guessedCountry.Success.Should().BeFalse();
        guessedCountry.IsSameContinent.Should().BeFalse();
        guessedCountry.Direction.Should().Be(104);
        guessedCountry.Kilometers.Should().Be(6843);
        guessedCountry.Flag.Uri.Should().Be(_canada.Resources.Flag.Uri);
        guessedCountry.Flag.MediaType.Should().Be(_canada.Resources.Flag.MediaType);
    }

    [Fact]
    public async Task HandleShouldReturnTheCountryWhenIsSameCountry()
    {
        GuessCountry.Command command = new(_canada.Cca2, _canada.Cca2);

        GuessedCountryResponse guessedCountry = await _handler.Handle(command, CancellationToken.None);

        guessedCountry.Cca2.Should().Be(_canada.Cca2);
        guessedCountry.Name.Should().Be(_canada.Translations.First().Name);
        guessedCountry.Success.Should().BeTrue();
        guessedCountry.IsSameContinent.Should().BeTrue();
        guessedCountry.Direction.Should().Be(0);
        guessedCountry.Kilometers.Should().Be(0);
        guessedCountry.Flag.Uri.Should().Be(_canada.Resources.Flag.Uri);
        guessedCountry.Flag.MediaType.Should().Be(_canada.Resources.Flag.MediaType);
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = [],
        Capitals = [],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, -95),
        Population = 38005238,
        Translations = [new Translation(Language.English, "Canada")],
        IsExcluded = false,
        Resources = new CountryResources()
        {
            Map = new Uri("https://www.google.com/maps/place/Canada"),
            Flag = new Image(new Uri("https://www.countryflags.io/ca/flat/64.svg"), "svg")
        }
    };

    private static Country CreateItaly() => new()
    {
        Cca2 = new Cca2("IT"),
        Area = new Area(301336),
        Borders = [],
        Capitals = [],
        Continent = Continent.Europe,
        Coordinate = new Coordinate(42.83333333, 12.83333333),
        Population = 59554023,
        Translations = [new Translation(Language.English, "Italy")],
        IsExcluded = false,
        Resources = new CountryResources()
        {
            Map = new Uri("https://www.google.com/maps/place/Italy"),
            Flag = new Image(new Uri("https://www.countryflags.io/it/flat/64.svg"), "svg")
        }
    };
}
