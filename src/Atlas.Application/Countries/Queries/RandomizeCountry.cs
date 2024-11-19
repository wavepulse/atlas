// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Responses.Mappers;
using Atlas.Application.Services;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class RandomizeCountry
{
    public sealed record Query : IQuery<CountryResponse>;

    internal sealed class Handler(IRandomizer randomizer, ICountryRepository repository) : IQueryHandler<Query, CountryResponse>
    {
        public async ValueTask<CountryResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            ReadOnlySpan<Country> countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            Country randomizedCountry = randomizer.Randomize(countries);

            repository.Cache(randomizedCountry);

            return randomizedCountry.ToResponse();
        }
    }
}
