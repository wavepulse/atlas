// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Services;
using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class RandomizeCountry
{
    public sealed record Query : IQuery<RandomizedCountryResponse>;

    internal sealed class Handler(IRandomizer randomizer, ICountryRepository repository) : IQueryHandler<Query, RandomizedCountryResponse>
    {
        public async ValueTask<RandomizedCountryResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            Country[] countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            Country randomizedCountry = randomizer.Randomize<Country>(countries);

            repository.Cache(randomizedCountry);

            return ToResponse(randomizedCountry);
        }

        private static RandomizedCountryResponse ToResponse(Country country)
        {
            string name = country.Translations.First(t => t.Language == Language.English).Name;
            (Uri map, Image flag) = country.Resources;

            return new(country.Cca2, name, new ImageResponse(flag.Uri, flag.MediaType), map);
        }
    }
}
