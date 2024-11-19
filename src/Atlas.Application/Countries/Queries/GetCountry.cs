// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Responses.Mappers;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class GetCountry
{
    public sealed record Query(string Cca2) : IQuery<CountryResponse>;

    internal sealed class Handler(ICountryRepository repository) : IQueryHandler<Query, CountryResponse>
    {
        public async ValueTask<CountryResponse> Handle(Query query, CancellationToken cancellationToken)
        {
            Country? country = await repository.GetAsync(new Cca2(query.Cca2), cancellationToken).ConfigureAwait(false);

            return country!.ToResponse();
        }
    }
}
