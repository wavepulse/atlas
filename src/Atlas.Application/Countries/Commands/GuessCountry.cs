// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Mediator;

namespace Atlas.Application.Countries.Commands;

public static class GuessCountry
{
    public sealed record Command(string GuessedCca2, string RandomizedCca2) : ICommand<GuessedCountryResponse>;

    internal sealed class Handler(ICountryRepository repository) : ICommandHandler<Command, GuessedCountryResponse>
    {
        public async ValueTask<GuessedCountryResponse> Handle(Command command, CancellationToken cancellationToken)
        {
            Country? country = await repository.GetAsync(new Cca2(command.RandomizedCca2), cancellationToken).ConfigureAwait(false);
            Country? guessedCountry = await repository.GetAsync(new Cca2(command.GuessedCca2), cancellationToken).ConfigureAwait(false);

            return Guess(country!, guessedCountry!);
        }

        private static GuessedCountryResponse Guess(Country country, Country guessedCountry) => new()
        {
            Cca2 = guessedCountry.Cca2,
            Success = country.Cca2 == guessedCountry.Cca2,
            Name = guessedCountry.Translations.First(t => t.Language == Language.English).Name,
            Direction = Direction.Calculate(guessedCountry.Coordinate, country.Coordinate),
            Kilometers = (int)Math.Round(Distance.Calculate(guessedCountry.Coordinate, country.Coordinate).Kilometers),
            IsSameContinent = guessedCountry.Continent == country.Continent,
            Flag = new ImageResponse(guessedCountry.Resources.Flag.Uri, guessedCountry.Resources.Flag.MediaType)
        };
    }
}
