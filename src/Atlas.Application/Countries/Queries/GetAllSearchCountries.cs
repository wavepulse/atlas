// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Mediator;
using SearchCountryResponse = Atlas.Contracts.Countries.SearchCountry;

namespace Atlas.Application.Countries.Queries;

public static class GetAllSearchCountries
{
    public record Query : IQuery<SearchCountryResponse[]>;

    internal sealed class Handler(ISearchCountryRepository repository) : IQueryHandler<Query, SearchCountryResponse[]>
    {
        public async ValueTask<SearchCountryResponse[]> Handle(Query query, CancellationToken cancellationToken)
        {
            SearchCountry[] countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return countries.Select(Map).ToArray();
        }

        private static SearchCountryResponse Map(SearchCountry country)
        {
            (_, string name) = country.Translations.First(t => t.Code.Equals("eng", StringComparison.OrdinalIgnoreCase));

            return new SearchCountryResponse(country.Cca2, name);
        }
    }
}
