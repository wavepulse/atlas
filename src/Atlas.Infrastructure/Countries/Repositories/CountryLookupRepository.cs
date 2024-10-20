// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Countries.Sources;

namespace Atlas.Infrastructure.Countries.Repositories;

internal sealed class CountryLookupRepository(IDataSource<CountryLookup> dataSource) : ICountryLookupRepository
{
    public async Task<CountryLookup[]> LookupAsync(CancellationToken cancellationToken)
    {
        CountryLookup[] countries = await dataSource.QueryAllAsync(cancellationToken).ConfigureAwait(false);

        return countries.Where(c => !c.IsExcluded).ToArray();
    }
}
