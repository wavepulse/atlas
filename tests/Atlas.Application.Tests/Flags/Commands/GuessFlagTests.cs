// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Contracts.Flags;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;

namespace Atlas.Application.Flags.Commands;

public sealed class GuessFlagTests
{
    private readonly Country _canada = CreateCanada();
    private readonly Country _italy = CreateItaly();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GuessFlag.Handler _handler;

    public GuessFlagTests()
    {
        _repository.GetByCodeAsync(_canada.Cca2, CancellationToken.None).Returns(_canada);
        _repository.GetByCodeAsync(_italy.Cca2, CancellationToken.None).Returns(_italy);

        _handler = new GuessFlag.Handler(_repository);
    }

    [Fact]
    public async Task HandleShouldReturnTheGuessedFlagWhenIsNotSameCountry()
    {
        GuessFlag.Command command = new(_canada.Cca2, _italy.Cca2);

        GuessedFlag guessedFlag = await _handler.Handle(command, CancellationToken.None);

        guessedFlag.Cca2.Should().Be(_canada.Cca2);
        guessedFlag.Name.Should().Be(_canada.Translations.First().Name);
        guessedFlag.Success.Should().BeFalse();
        guessedFlag.IsSameContinent.Should().BeFalse();
        guessedFlag.Direction.Should().Be(104);
        guessedFlag.Kilometers.Should().Be(6843);
    }

    [Fact]
    public async Task HandleShouldReturnTheCountryWhenIsSameCountry()
    {
        GuessFlag.Command command = new(_canada.Cca2, _canada.Cca2);

        GuessedFlag guessedFlag = await _handler.Handle(command, CancellationToken.None);

        guessedFlag.Cca2.Should().Be(_canada.Cca2);
        guessedFlag.Name.Should().Be(_canada.Translations.First().Name);
        guessedFlag.Success.Should().BeTrue();
        guessedFlag.IsSameContinent.Should().BeTrue();
        guessedFlag.Direction.Should().Be(0);
        guessedFlag.Kilometers.Should().Be(0);
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = "CA",
        Area = new Area(9984670),
        Borders = [],
        Capitals = [],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, -95),
        Population = 38005238,
        Translations = [new Translation("eng", "Canada")]
    };

    private static Country CreateItaly() => new()
    {
        Cca2 = "IT",
        Area = new Area(301336),
        Borders = [],
        Capitals = [],
        Continent = Continent.Europe,
        Coordinate = new Coordinate(42.83333333, 12.83333333),
        Population = 59554023,
        Translations = [new Translation("eng", "Italy")]
    };
}
