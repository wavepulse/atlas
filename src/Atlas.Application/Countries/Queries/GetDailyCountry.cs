// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Responses.Mappers;
using Atlas.Application.Services;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class GetDailyCountry
{
    public sealed record Query : IQuery<RandomizedCountryResponse>;

    internal sealed class Handler(IDateHash dateHash, ICountryRepository countryRepository, ITimeService timeService) : IQueryHandler<Query, RandomizedCountryResponse>
    {
        public async ValueTask<RandomizedCountryResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            uint hash = dateHash.Hash(timeService.Today);

            Country[] countries = await countryRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            Country country = countries[hash % countries.Length];

            countryRepository.Cache(country);

            return country.ToResponse();
        }
    }
}
