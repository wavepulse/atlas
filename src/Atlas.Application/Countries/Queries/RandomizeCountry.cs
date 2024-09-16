// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Services;
using Atlas.Contracts.Countries;
using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class RandomizeCountry
{
    public sealed record Query : IQuery<RandomizedCountry>;

    internal sealed class Handler(IRandomizer randomizer, ICountryRepository repository) : IQueryHandler<Query, RandomizedCountry>
    {
        public async ValueTask<RandomizedCountry> Handle(Query query, CancellationToken cancellationToken)
        {
            Country[] countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            Country randomizedCountry = randomizer.Randomize<Country>(countries);

            string name = randomizedCountry.Translations.First(t => t.Language == Language.English).Name;

            return new RandomizedCountry(randomizedCountry.Cca2, name, randomizedCountry.FlagSvgUri, randomizedCountry.MapUri);
        }
    }
}
