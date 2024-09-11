// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Contracts.Flags;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Mediator;

namespace Atlas.Application.Flags.Commands;

public static class GuessFlag
{
    public sealed record Command(string GuessedCca2, string Cca2) : ICommand<GuessedFlag>;

    internal sealed class Handler(ICountryRepository repository) : ICommandHandler<Command, GuessedFlag>
    {
        public async ValueTask<GuessedFlag> Handle(Command command, CancellationToken cancellationToken)
        {
            Country? country = await repository.GetByCodeAsync(command.Cca2, cancellationToken).ConfigureAwait(false);
            Country? guessedCountry = await repository.GetByCodeAsync(command.GuessedCca2, cancellationToken).ConfigureAwait(false);

            return Guess(country!, guessedCountry!);
        }

        private static GuessedFlag Guess(Country country, Country guessedCountry) => new()
        {
            Cca2 = country.Cca2,
            Success = string.Equals(country.Cca2, guessedCountry.Cca2, StringComparison.OrdinalIgnoreCase),
            Name = guessedCountry.Translations.First().Name,
            Direction = Direction.Calculate(guessedCountry.Coordinate, country.Coordinate),
            Kilometers = Distance.Calculate(guessedCountry.Coordinate, country.Coordinate).Kilometers,
            IsSameContinent = guessedCountry.Continent == country.Continent
        };
    }
}
