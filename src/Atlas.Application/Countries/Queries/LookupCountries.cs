// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class LookupCountries
{
    public sealed record Query : IQuery<CountryLookupResponse[]>;

    internal sealed class Handler(ICountryLookupRepository repository) : IQueryHandler<Query, CountryLookupResponse[]>
    {
        public async ValueTask<CountryLookupResponse[]> Handle(Query query, CancellationToken cancellationToken)
        {
            CountryLookup[] countries = await repository.LookupAsync(cancellationToken).ConfigureAwait(false);

            return countries.Select(Map).ToArray();
        }

        private static CountryLookupResponse Map(CountryLookup country)
        {
            (_, string name) = country.Translations.First(t => t.Language == Language.English);

            return new CountryLookupResponse(country.Cca2, name);
        }
    }
}
